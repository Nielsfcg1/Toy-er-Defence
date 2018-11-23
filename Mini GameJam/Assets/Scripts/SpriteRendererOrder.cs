using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererOrder : MonoBehaviour {

	SpriteRenderer sr;
	SpriteRenderer playerSpriteRenderer;

	public enum RenderMode {auto, alwaysAbovePlayer, alwaysBelowPlayer, customLayer}
	public RenderMode mode;

    public int customLayer;

	void Start () {
		sr = GetComponent<SpriteRenderer>();
		playerSpriteRenderer = PlayerChar.Instance.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {


		if (mode == RenderMode.auto)
		{
			sr.sortingOrder = (int)(-transform.position.z * 100);
		}
		else if (mode == RenderMode.alwaysAbovePlayer)
		{
			sr.sortingOrder = playerSpriteRenderer.sortingOrder + 10;
		}
		else if (mode == RenderMode.alwaysBelowPlayer)
		{
			sr.sortingOrder = playerSpriteRenderer.sortingOrder - 10;
		}
        else if (mode == RenderMode.customLayer)
        {
            sr.sortingOrder = customLayer;
        }
	}
}
