using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOut : MonoBehaviour {

	public enum Types{Text3D, SpriteRenderer};
	public Types type;

	[Header("Fade")]
	public float speed = 0.1f;
	public float fadeAmount = 0.2f;

	[Header("Movement")]
	public bool moveUp = true;
	public float moveUpAmount = 0.7f;

	void Start() {
		switch (type) {
		case Types.Text3D:
			StartCoroutine (Fade3DText (moveUp));
			break;
		case Types.SpriteRenderer:
			StartCoroutine (FadeSpriterRenderer (moveUp));
			break;
		}

	}

	IEnumerator Fade3DText(bool moveUp) {
		TextMesh text = GetComponent<TextMesh> ();

		while (text.color.a >= 0) {
			Color newColor = new Color (text.color.r, text.color.g, text.color.b, text.color.a - fadeAmount);
			text.color = newColor;

			if (moveUp) {
				transform.Translate (new Vector3 (0f, moveUpAmount, 0f));
			}

			if (text.color.a == 0) {
				Destroy (gameObject);
			}

			yield return new WaitForSeconds (speed);
		}
	}
	IEnumerator FadeSpriterRenderer(bool moveUp) {
		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();

		while (sprite.color.a >= 0) {
			Color newColor = new Color (sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - fadeAmount);
			sprite.color = newColor;

			if (moveUp) {
				transform.Translate (new Vector3 (0f, moveUpAmount, 0f));
			}

			if (sprite.color.a == 0) {
				Destroy (gameObject);
			}

			yield return new WaitForSeconds (speed);
		}
	}
}