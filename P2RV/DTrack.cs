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
		/*p.m22 = 0.0f;
		p.m00 = -1.0f;
		p.m12 = -1.0f;
		p.m21 = 1.0f;
		p.m11 = 0.0f;*/
		inv_p = p.inverse;
	}

	// Update is called once per frame
	void Update()
	{
		while(udpClient.Available > 0)
		{
			IPEndPoint ep = null;

			byte[] bytes;

			bytes = udpClient.Receive(ref ep);

			if(bytes != null)
			{
				string s = System.Text.Encoding.Default.GetString(bytes);
                Debug.Log(s);

				if(!string.IsNullOrEmpty(s))
				{
                    // example of frame 
                    //fr 7780 ts 43073.420053 6d 1[0 1.000][120.015 294.414 1116.173 162.7024 - 35.9093 147.6201][-0.684013 - 0.364036 0.632143 - 0.433750 0.899712 0.048781 - 0.586504 - 0.240826 - 0.773315]
                    //fr 95808 ts 44540.369881 6d 1[0 1.000][297.081 439.608 1019.273 89.7821 - 14.3863 - 4.4871][0.965674 - 0.247992 - 0.077293 0.075782 - 0.015646 0.997002 - 0.248458 - 0.968636 0.003684]
                    //fr 101110  ts 44628.725493 6d 2[0 1.000][394.253 432.861 1018.120 90.2882 - 30.5219 - 3.8462][0.859495 - 0.506380 - 0.069627 0.057784 - 0.039086 0.997564 - 0.507868 - 0.861424 - 0.004333][1 1.000][228.218 529.235 1016.346 - 104.9997 31.0162 - 172.3396][-0.849373 0.527782 - 0.003413 0.114242 0.190157 0.975084 0.515281 0.827820 - 0.221809]


                    char[] separators = { ' ', '[', ']', '\r', '\n' };

					string[] toto = s.Split(separators);

                    int index_6d = -1;

					for(int i = 0; i < toto.Length; i++)
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

						current_index++;
						body.id = int.Parse(toto[current_index]);

						current_index++;

						body.quality = float.Parse(toto[current_index]);

						current_index += 2;

						Matrix4x4 m = Matrix4x4.identity;

						m.m03 = 0.001f * float.Parse(toto[current_index]);

						current_index++;

						m.m13 = 0.001f * float.Parse(toto[current_index]);

						current_index++;

						m.m23 = 0.001f * float.Parse(toto[current_index]);

						current_index += 5;


						m.m00 = float.Parse(toto[current_index]);
						current_index++;
						m.m10 = float.Parse(toto[current_index]);
						current_index++;
						m.m20 = float.Parse(toto[current_index]);
						current_index++;
						m.m01 = float.Parse(toto[current_index]);
						current_index++;
						m.m11 = float.Parse(toto[current_index]);
						current_index++;
						m.m21 = float.Parse(toto[current_index]);
						current_index++;
						m.m02 = float.Parse(toto[current_index]);
						current_index++;
						m.m12 = float.Parse(toto[current_index]);
						current_index++;
						m.m22 = float.Parse(toto[current_index]);

						Matrix4x4 a = p * m * inv_p; // on passe la matrice du repère de la salle au repère de Unity

						//body.position = a.ExtractPosition();
                        Vector3 position;
                        position.x = a.m03;
                        position.y = a.m13;
                        position.z = a.m23;
                        body.position = position;
                        /*float temp = body.position.y;
						body.position.y = -body.position.z;
						body.position.z = temp;*/
                        //body.orientation = a.ExtractQuaternion();
                        Vector3 forward;
                        forward.x = a.m02;
                        forward.y = a.m12;
                        forward.z = a.m22;

                        Vector3 upwards;
                        upwards.x = a.m01;
                        upwards.y = a.m11;
                        upwards.z = a.m21;

                        body.orientation = Quaternion.LookRotation(forward, upwards);

                        body.rotationMatrix = a;

						bodies[body.id] = body;

						current_index += 2;
					}
				}
			}
		}
	}

	public bool GetBody(int id, out BodyDesc body)
	{
		body = new BodyDesc();

		if(bodies != null)
		{
			return bodies.TryGetValue(id, out body);
		}
		else
		{
			return false;
		}
	}
}
