using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class DTrack : MonoBehaviour
{
    public struct BodyDesc
    {
        public int id;
        public float quality;
        public Vector3 position;
        public Quaternion orientation;
        public Matrix4x4 rotationMatrix;
    }

    public int port = 5000;

    UdpClient udpClient;

    Dictionary<int, BodyDesc> bodies;

    Matrix4x4 p, inv_p;


    // Use this for initialization
    void Start()
    {
        udpClient = new UdpClient(port);

        p = Matrix4x4.identity;
        inv_p = p.inverse;
    }

    void skipEmptyValue(string[] s, ref int index)
    {
        while ((s[index] == null) || (s[index].Length ==0))
        {
            index++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        while (udpClient.Available > 0)
        {
            IPEndPoint ep = null;

            byte[] bytes;

            bytes = udpClient.Receive(ref ep);

            if (bytes != null)
            {
                string s = System.Text.Encoding.Default.GetString(bytes);
  
                Debug.Log(System.Text.RegularExpressions.Regex.Unescape(s));

                if (!string.IsNullOrEmpty(s))
                {
                    
                    char[] separators = { ' ', '[', ']', '\r', '\n', '\t'};

                    string[] toto = s.Split(separators);
      
                   
                    int index_6d = -1;

                    for (int i = 0; i < toto.Length; i++)
                    {
                        if (toto[i] == "6d")
                        {
                            index_6d = i;
                         
                            break;
                        }
                    }

                    // number of bodies detected in the frame
                    int count = int.Parse(toto[index_6d + 1]);

                    bodies = new Dictionary<int, BodyDesc>();
                    
                    int current_index = index_6d + 2;

                    for (int i = 0; i < count; i++)
                    {
                        BodyDesc body = new BodyDesc();

                        //current_index++;
                        skipEmptyValue(toto, ref current_index);
                        //Debug.Log("bodyId"+toto[current_index]);

                        body.id = int.Parse(toto[current_index]);

                        
                        current_index++;

                        skipEmptyValue(toto, ref current_index);
                        
                        body.quality = float.Parse(toto[current_index], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);

                        //current_index += 2;
                        current_index ++;
                        skipEmptyValue(toto, ref current_index);

                        Matrix4x4 m = Matrix4x4.identity;

                        m.m03 = 0.001f * float.Parse(toto[current_index], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);

                        current_index++;
                        skipEmptyValue(toto, ref current_index);

                        m.m13 = 0.001f * float.Parse(toto[current_index], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);

                        current_index++;
                        skipEmptyValue(toto, ref current_index);

                        m.m23 = 0.001f * float.Parse(toto[current_index], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);

                        //current_index += 5;
                        for(int g=0;g<4;g++)
                        {
                            current_index++;
                            skipEmptyValue(toto, ref current_index);
                        }


                        m.m00 = float.Parse(toto[current_index], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                        current_index++;
                        skipEmptyValue(toto, ref current_index);
                        m.m10 = float.Parse(toto[current_index], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                        current_index++;
                        skipEmptyValue(toto, ref current_index);
                        m.m20 = float.Parse(toto[current_index], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                        current_index++;
                        skipEmptyValue(toto, ref current_index);
                        m.m01 = float.Parse(toto[current_index], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                        current_index++;
                        skipEmptyValue(toto, ref current_index);
                        m.m11 = float.Parse(toto[current_index], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                        current_index++;
                        skipEmptyValue(toto, ref current_index);
                        m.m21 = float.Parse(toto[current_index], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                        current_index++;
                        m.m02 = float.Parse(toto[current_index], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                        current_index++;
                        skipEmptyValue(toto, ref current_index);
                        m.m12 = float.Parse(toto[current_index], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                        current_index++;
                        skipEmptyValue(toto, ref current_index);
                        m.m22 = float.Parse(toto[current_index], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                        
                        // ignore this line 
                       Matrix4x4 a = p * m * inv_p; 
              

                        // position dtrack to unity
                        Vector3 position;
                        
                        position.x = a.m03;
                        position.y = a.m23;//change y and z
                        position.z = a.m13;
                     
                        body.position = position;

                        // rotation dtrack to unity
                        float t;
                        float x, y, z, w;

                        if (a.m22 < 0.0f)
                        {
                            if (a.m00 > a.m11)
                            {
                                t = 1.0f + a.m00 - a.m11 - a.m22;
                                x = t; y = a.m01 + a.m10; z = a.m20 + a.m02; w = a.m12 - a.m21;
                            }
                            else
                            {
                                t = 1.0f - a.m00 + a.m11 - a.m22;
                                x = a.m01 + a.m10; y = t; z = a.m12 + a.m21; w = a.m20 - a.m02;
                            }
                        }
                        else
                        {
                            if (a.m00 < -a.m11)
                            {
                                t = 1.0f - a.m00 - a.m11 + a.m22;
                                x = a.m20 + a.m02; y = a.m12 + a.m21; z = t; w = a.m01 - a.m10;
                            }
                            else
                            {
                                t = 1.0f + a.m00 + a.m11 + a.m22;
                                x = a.m12 - a.m21; y = a.m20 - a.m02; z = a.m01 - a.m10; w = t;
                            }
                        }

                        float q = 0.5f / Mathf.Sqrt(Mathf.Max(0.0f, t));

                        body.orientation = new Quaternion(q * x, q * z, q * y, q * w);


                        body.rotationMatrix = a;

                        bodies[body.id] = body;

                        //current_index += 2;
                        current_index++;
                    }
                }
            }
        }
    }

    public bool GetBody(int id, out BodyDesc body)
    {
        body = new BodyDesc();

        if (bodies != null)
        {
            return bodies.TryGetValue(id, out body);
        }
        else
        {
            return false;
        }
    }
}
