using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace myengine.BlockPuzzle
{
    public class TileObject : MonoBehaviour
    {
        public PieceDisplay model;
        private Vector2 inital_model_pos;
        public GameObject greyshit;
        public PieceData _data;
        public PieceData fakeData;
        public Vector2 position;
        public GameObject flashbang;
        [SerializeField] private float shatterSpeed;

        private void OnEnable()
        {
            Color tmp = greyshit.gameObject.GetComponent<SpriteRenderer>().color;
            tmp.a = 0f;
            greyshit.gameObject.GetComponent<SpriteRenderer>().color = tmp;
            inital_model_pos = model.gameObject.transform.localPosition;
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
            Tween tween1 = model.gameObject.transform.DOLocalMove(new Vector2(0, 0.5f), 0.25f).SetUpdate(true);
            Tween tween2 = model.gameObject.GetComponent<SpriteRenderer>().DOFade(0f, 0.25f).SetUpdate(true);
            finSeq.Append(tween1);
            finSeq.Join(tween2);
            _data = null;
            fakeData = null;
            finSeq.AppendCallback(delegate
            {
                model.gameObject.transform.DOLocalMove(new Vector2(0, 0), 0.1f).SetUpdate(true);
                model.gameObject.GetComponent<SpriteRenderer>().DOFade(1f, 0.1f).SetUpdate(true);
                model.gameObject.SetActive(false);
            });
        }

        public void SpinnyFinish(int index)
        {
            UnFlash();
            Sequence spinnySeq = DOTween.Sequence();
            spinnySeq.PrependInterval(0.05f * index);
            Tween tween1 = model.gameObject.transform.DOLocalRotate(new Vector3(0, 0, 180f), 0.5f).SetUpdate(true);
            Tween tween2 = model.gameObject.transform.DOScale(0f, 0.5f).SetUpdate(true);
            Tween tween3 = model.gameObject.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f).SetUpdate(true);
            spinnySeq.Append(tween1);
            spinnySeq.Join(tween2);
            spinnySeq.Join(tween3);
            _data = null;
            fakeData = null;
            spinnySeq.AppendCallback(delegate
            {
                model.gameObject.transform.localRotation = Quaternion.identity;
                model.gameObject.transform.DOScale(1f, 0.1f).SetUpdate(true);
                model.gameObject.GetComponent<SpriteRenderer>().DOFade(1f, 0.1f).SetUpdate(true);
                model.gameObject.SetActive(false);
            });
        }

        //Unfin
        public void ShatterFinish()
        {
            UnFlash();
            _data = null;
            fakeData = null;
            Sequence shatterSeq = DOTween.Sequence();
            float upValue = 0.25f;
            float mixValue = Random.Range(-1f, 1f);
            Tween tween1 = model.transform.DOLocalMove(new Vector2(mixValue, upValue), 0.25f);
            Tween tween2 = model.transform.DOLocalRotate(new Vector3(0, 0, 180f), 0.75f);
            Tween tween3 = model.transform.DOScale(new Vector2(0.5f, 0.5f), 0.5f);
            //Tween tween4 = model.transform.DOLocalMove(new Vector2(Random.Range(-1f, 1f), -1f), 0.5f);
            shatterSeq.Insert(0, tween1);
            shatterSeq.Insert(0, tween2);
            shatterSeq.Insert(1, tween3);
            //shatterSeq.Insert(1, tween4);
            shatterSeq.AppendCallback(delegate
            {
                model.transform.localPosition = inital_model_pos;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = new Vector2(1, 1);
                model.gameObject.SetActive(false);
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
            greyshit.GetComponent<SpriteRenderer>().DOFade(1f, 0.5f).SetUpdate(true);
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
