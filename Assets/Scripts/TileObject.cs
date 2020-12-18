using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace myengine.BlockPuzzle
{
    public class TileObject : MonoBehaviour
    {
        public PieceDisplay model;
        public GameObject greyshit;
        public PieceData _data;
        public PieceData fakeData;
        public Vector2 position;
        public GameObject flashbang;

        private void OnEnable()
        {
            Color tmp = greyshit.gameObject.GetComponent<SpriteRenderer>().color;
            tmp.a = 0f;
            greyshit.gameObject.GetComponent<SpriteRenderer>().color = tmp;
        }

        public void AddPieceData(PieceData data)
        {
            model.LoadData(data);
            model.gameObject.SetActive(data != null);
            Color tmp = model.gameObject.GetComponent<SpriteRenderer>().color;
            tmp.a = 1f;
            model.gameObject.GetComponent<SpriteRenderer>().color = tmp;
            _data = data;
        }

        public void Hovering(PieceData data)
        {
            model.LoadData(data);
            fakeData = data;
            model.gameObject.SetActive(data != null);
            Color tmp = model.gameObject.GetComponent<SpriteRenderer>().color;
            tmp.a = 0.6f;
            model.gameObject.GetComponent<SpriteRenderer>().color = tmp;
        }

        public void UnHover()
        {
            fakeData = null;
            model.gameObject.SetActive(false);
            flashbang.SetActive(false);
        }

        public void DragFinish(int index)
        {
            UnFlash();
            Sequence finSeq = DOTween.Sequence();
            finSeq.PrependInterval(0.05f * index);
            Tween tween1 = model.gameObject.transform.DOLocalMove(new Vector2(0, 0.5f), 0.25f);
            Tween tween2 = model.gameObject.GetComponent<SpriteRenderer>().DOFade(0f, 0.25f);
            finSeq.Append(tween1);
            finSeq.Join(tween2);
            finSeq.AppendCallback(delegate
            {
                model.gameObject.transform.DOLocalMove(new Vector2(0, 0), 0.1f);
                model.gameObject.GetComponent<SpriteRenderer>().DOFade(1f, 0.1f);
                model.gameObject.SetActive(false);
                _data = null;
                fakeData = null;
            });
        }

        public void SpinnyFinish(int index)
        {
            UnFlash();
            Sequence spinnySeq = DOTween.Sequence();
            spinnySeq.PrependInterval(0.05f * index);
            Tween tween1 = model.gameObject.transform.DOLocalRotate(new Vector3(0, 0, 180f), 0.5f);
            Tween tween2 = model.gameObject.transform.DOScale(0f, 0.5f);
            Tween tween3 = model.gameObject.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f);
            spinnySeq.Append(tween1);
            spinnySeq.Join(tween2);
            spinnySeq.Join(tween3);
            spinnySeq.AppendCallback(delegate
            {
                model.gameObject.transform.rotation = Quaternion.identity;
                model.gameObject.transform.DOScale(1f, 0.1f);
                model.gameObject.GetComponent<SpriteRenderer>().DOFade(1f, 0.1f);
                model.gameObject.SetActive(false);
                _data = null;
                fakeData = null;
            });
        }

        public void Flash()
        {
            if (!flashbang.activeSelf)
            {
                flashbang.SetActive(true);
            }
        }

        public void UnFlash()
        {
            if (flashbang.activeSelf)
            {
                flashbang.SetActive(false);
            }
        }

        public bool isEmptyHover()
        {
            if (fakeData == null)
            {
                return true;
            }
            return false;
        }

        public bool isEmpty()
        {
            if (_data == null)
            {
                return true;
            }
            return false;
        }

        public void Freeze()
        {
            greyshit.GetComponent<SpriteRenderer>().DOFade(1f, 0.5f);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
