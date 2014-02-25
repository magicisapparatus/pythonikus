/* skybox.cs		Skybox Script
 * author:			SoulofDeity
 * 
 * Features:
 *	- animation via rotation on y axis, speed adjustable
 *  - skybox tinting (requires skybox shader)
 * 	- smooth fading color transitioning over a specified
 *    period of time
 *  - multiple skyboxes
 *  - smooth fading skybox transitioning over a specified
 *    period of time (works in conjunction with color
 *    fading as well)
 * 
 * Technical Details:
 *	- uses user layer 8 for the skybox layer, which is
 *    rendered at a depth of -1
 *	- gSkybox is the global declaration of the skybox
 *    transform
 *  - tSkybox is the global declaration of the target
 *    skybox transform used when transitioning
 *  - skyboxColorTrans tells whether or not the skybox
 *    is transitioning from one color to another. you
 *    cannot transition to another color while this is
 *    happening.
 *  - skyboxTexTrans tells whether or not the skybox
 *    is transitioning from one set of textures to
 *    another. you cannot transition to another set of
 *    textures while this is happening.
 *  - skybox texture arrays are stored in the order:
 *       front, back, left, right, top, bottom
 ***************************************************************/
using UnityEngine;
using System.Collections;


public class skybox : MonoBehaviour {
	enum SkyboxType {
		DAY			= 0,
		NIGHT		= 1
	};

	static Transform gSkybox;
	static Transform tSkybox		= null;
	static bool skyboxColorTrans	= false;
	static bool skyboxTexTrans		= false;

	public float rotationSpeed = 5.0f;
	public Shader shader;
	public Color hue = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	public Texture[] daySkybox		= new Texture[6];
	public Texture[] nightSkybox	= new Texture[6];
	
	private Mesh cubeMesh;

	
	void Start() {
		initCubeMesh();
		
		Camera camera = (Camera)transform.GetComponent("Camera");
		camera.clearFlags = CameraClearFlags.Depth;
		camera.cullingMask = ~(1 << 8);
		camera.depth = 0;
			
		Transform skyboxCam = (new GameObject("skyboxCam")).transform;
		Camera cam = (Camera)skyboxCam.gameObject.AddComponent("Camera");
		cam.enabled = true;
		cam.clearFlags = CameraClearFlags.SolidColor;
		cam.cullingMask = 1 << 8;
		cam.depth = -1;

		gSkybox = createSkybox("skybox", daySkybox);
		gSkybox.parent = skyboxCam;
	}
	
	void Update() {
		gSkybox.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
		if (tSkybox) tSkybox.rotation = gSkybox.rotation;
		if (Input.GetKeyDown(KeyCode.Z)) {
			Transition(SkyboxType.DAY, Color.white, 3.0f);
		} else if (Input.GetKeyDown(KeyCode.X)) {
			Transition(SkyboxType.DAY, new Color(0.75f, 0.15f, 0.0f, 1.0f), 3.0f);
		} else if (Input.GetKeyDown(KeyCode.C)) {
			Transition(SkyboxType.NIGHT, new Color(0.75f, 0.75f, 1.0f, 1.0f), 3.0f);
		}
	}

	
	IEnumerator crTransition(Color color, float time) {
		if (skyboxColorTrans) yield break;
		float i = 0.0f;
		MeshRenderer gmr = (MeshRenderer)gSkybox.GetComponent("MeshRenderer");
		Color start = gmr.materials[0].color;
		skyboxColorTrans = true;
		while (i <= 1.0f) {
			Color tc = Color.Lerp(start, color, i);
			gmr.materials[0].color = new Color(tc.r, tc.g, tc.b, gmr.materials[0].color.a);
			for (int j = 1; j < 6; j++)
				gmr.materials[j].color = gmr.materials[0].color;
			i += Time.deltaTime / time;
			yield return null;
		}
		skyboxColorTrans = false;
	}
	
	IEnumerator crTransition(SkyboxType type, float time) {
		if (skyboxTexTrans) yield break;
		Texture[] textures;
		switch (type) {
			case SkyboxType.NIGHT: {
				textures = nightSkybox;
				break;
			}
			default: {
				textures = daySkybox;
				break;
			}
		}
		tSkybox = createSkybox("skybox", textures);
		tSkybox.parent = gSkybox.parent;
		MeshRenderer gmr = (MeshRenderer)gSkybox.GetComponent("MeshRenderer");
		MeshRenderer tmr = (MeshRenderer)tSkybox.GetComponent("MeshRenderer");
		Color color = new Color(
			gmr.materials[0].color.r,
			gmr.materials[0].color.g,
			gmr.materials[0].color.b,
			0.0f);
		for (int i = 0; i < 6; i++)
			tmr.materials[i].color = color;
		skyboxTexTrans = true;
		float j = 0.0f;
		while (j <= 1.0f) {
			gmr.materials[0].color = new Color(
				gmr.materials[0].color.r,
				gmr.materials[0].color.g,
				gmr.materials[0].color.b,
				1.0f - j);
			tmr.materials[0].color = new Color(
				gmr.materials[0].color.r,
				gmr.materials[0].color.g,
				gmr.materials[0].color.b,
				j);
			for (int i = 1; i < 6; i++) {
				gmr.materials[i].color = gmr.materials[0].color;
				tmr.materials[i].color = tmr.materials[0].color;
			}
			j += Time.deltaTime / time;
			yield return null;
		}
		tmr.materials[0].color = new Color(
			gmr.materials[0].color.r,
			gmr.materials[0].color.g,
			gmr.materials[0].color.b,
			1.0f);
		for (int i = 1; i < 6; i++)
			tmr.materials[i].color = tmr.materials[0].color;
		GameObject.Destroy(gSkybox.gameObject);
		gSkybox = tSkybox;
		tSkybox = null;
		skyboxTexTrans = false;
	}

	void Transition(Color color, float time) {
		StartCoroutine(crTransition(color, time));
	}

	void Transition(SkyboxType type, float time) {
		StartCoroutine(crTransition(type, time));
	}
	
	void Transition(SkyboxType type, Color color, float time) {
		StartCoroutine(crTransition(color, time));
		StartCoroutine(crTransition(type, time));
	}
	
	Transform createSkybox(string name, Texture[] textures) {
		Transform sb = (new GameObject(name)).transform;
		sb.gameObject.layer = 8;
		MeshFilter mf = (MeshFilter)sb.gameObject.AddComponent("MeshFilter");
		mf.mesh = cubeMesh;
		MeshRenderer mr = (MeshRenderer)sb.gameObject.AddComponent("MeshRenderer");
		mr.enabled = true;
		mr.castShadows = false;
		mr.receiveShadows = false;
		mr.materials = new Material[6];
		for (int i = 0; i < 6; i++) {
			mr.materials[i] = new Material(shader);
			mr.materials[i].shader = shader;
			mr.materials[i].color = hue;
			mr.materials[i].mainTexture = textures[i];
		}
		return sb;
	}
	
	void initCubeMesh() {
		cubeMesh = new Mesh();
		cubeMesh.vertices = new Vector3[] {
			new Vector3(-1,-1, 1),			// front
			new Vector3(-1, 1, 1),
			new Vector3( 1, 1, 1),
			new Vector3( 1,-1, 1),
			
			new Vector3(-1,-1,-1),			// back
			new Vector3(-1, 1,-1),
			new Vector3( 1, 1,-1),
			new Vector3( 1,-1,-1),
			
			new Vector3(-1,-1,-1),			// left
			new Vector3(-1, 1,-1),
			new Vector3(-1, 1, 1),
			new Vector3(-1,-1, 1),
			
			new Vector3( 1,-1,-1),			// right
			new Vector3( 1, 1,-1),
			new Vector3( 1, 1, 1),
			new Vector3( 1,-1, 1),
			
			new Vector3(-1, 1,-1),			// top
			new Vector3(-1, 1, 1),
			new Vector3( 1, 1, 1),
			new Vector3( 1, 1,-1),
			
			new Vector3(-1,-1,-1),			// bottom
			new Vector3(-1,-1, 1),
			new Vector3( 1,-1, 1),
			new Vector3( 1,-1,-1)
		};
		cubeMesh.uv = new Vector2[] {
			new Vector2(0, 0),				// front
			new Vector2(0, 1),
			new Vector2(1, 1),
			new Vector2(1, 0),

			new Vector2(1, 0),				// back
			new Vector2(1, 1),
			new Vector2(0, 1),
			new Vector2(0, 0),
			
			new Vector2(0, 0),				// left
			new Vector2(0, 1),
			new Vector2(1, 1),
			new Vector2(1, 0),
			
			new Vector2(1, 0),				// right
			new Vector2(1, 1),
			new Vector2(0, 1),
			new Vector2(0, 0),
			
			new Vector2(1, 0),				// top
			new Vector2(1, 1),
			new Vector2(0, 1),
			new Vector2(0, 0),
			
			new Vector2(1, 1),				// bottom
			new Vector2(1, 0),
			new Vector2(0, 0),
			new Vector2(0, 1)
		};
		cubeMesh.subMeshCount = 6;
		cubeMesh.SetTriangles(new int[] { 0, 1, 2, 2, 3, 0}, 0);	// front
		cubeMesh.SetTriangles(new int[] { 6, 5, 4, 4, 7, 6}, 1);	// back
		cubeMesh.SetTriangles(new int[] { 8, 9,10,10,11, 8}, 2);	// left
		cubeMesh.SetTriangles(new int[] {14,13,12,12,15,14}, 3);	// right
		cubeMesh.SetTriangles(new int[] {18,17,16,16,19,18}, 4);	// top
		cubeMesh.SetTriangles(new int[] {20,21,22,22,23,20}, 5);	// bottom
		cubeMesh.RecalculateNormals();
		cubeMesh.Optimize();
	}
}
