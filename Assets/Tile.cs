using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Tile : MonoBehaviour
{
    public TileState state { get; private set; }
    public TileCell cell { get; private set; }

    public int number { get; private set; }
   
    public bool locked { get; set; }
    private Image background;
    private TextMeshProUGUI text;
    private Collider2D TileCollider;
    public void SetState(TileState state, int number)
    {
        Debug.Log(state+" "+ number);
        this.state = state;
        this.number = number;
        background.color = state.backgroundColor;
        text.text = number.ToString();
    }

    private void Awake()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        TileCollider = GetComponentInChildren<Collider2D>();
    }

    public void Spawn(TileCell cell , Transform ButtonCreate)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = cell;
        this.cell.tile = this;
        
        transform.position = ButtonCreate.transform.position;
        transform.DOMove(cell.transform.position, 0.5f);
    }

    public void MoveTo(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = cell;
        this.cell.tile = this;
        StartCoroutine(Animate(cell.transform.position, false));
    }

    public void Merge(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = null;
        cell.tile.locked = true;
        StartCoroutine(Animate(cell.transform.position, true));
    }

    private IEnumerator Animate(Vector3 to, bool isMerging)
    {
        float elapsed = 0f;
        float duration = 0.1f;

        Vector3 from = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = to;

        if (isMerging == true)
        {
            Destroy(gameObject);
        }
    }

    public void DragMerge()
    {
        

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.CompareTag("Tile") && collider.gameObject != gameObject)
            {
                if (number == collider.gameObject.GetComponent<Tile>().number)
                {
                    this.cell = null;
                    //cell.tile.locked = true;
                    //int index = Mathf.Clamp(IndexOf(state) + 1, 0, tileStates.Length - 1);
                    collider.gameObject.GetComponent<Tile>().SetState(state, number+1);
                    Destroy(gameObject);
                }
            }
            else
            {
                transform.DOMove(cell.transform.position, 0.5f);
            }
        }
            
    }

    //private int IndexOf(TileState state)
    //{
    //    for (int i = 0; i < tileStates.Length; i++)
    //    {
    //        if (state == tileStates[i])
    //        {
    //            return i;
    //        }
    //    }

    //    return -1;
    //}

}
