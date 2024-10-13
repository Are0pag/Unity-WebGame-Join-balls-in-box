using UnityEngine;
using Game_Bonuses;
using UnityEngine.UI;
using TakeAim;

namespace UI_SkillPanel {
    public class SkillItem : MonoBehaviour {
        [SerializeField] private Bonuses _displayedBonuse;
        [SerializeField] private Button _skillButton;
        [SerializeField] private Image _skillImage;
        [SerializeField] private GameObject _bgImage;

        [SerializeField] private GameObject _hint;

        private void InitSelf() {
            _skillImage.fillAmount = 0;
            _skillButton.onClick.AddListener(() => {
                if (CanBonusGetActive()) {
                    _bgImage.SetActive(false);

                    AudioManager.Instanse.PlaySound(AudioManager.Instanse.GlassClip);

                    BonusManager.Instance.SetBonusState(_displayedBonuse, () => {
                        CurrencyManager.Instance.Subtract(GetBonusCost(_displayedBonuse));
                        BonusManager.Instance.RemoveBonuseFromActiveBonusesList(_displayedBonuse);
                        GameManager.Instance.ChancheGameState(GameState.Playing);
                        BonusManager.Instance.RunningBonus = Bonuses.Default;
                        _bgImage.SetActive(true);
                    });
                }
                else {
                    _hint.SetActive(true);
                    SkillHint.Instance.InitTopDown(_displayedBonuse);
                }
            });
        }

        private bool CanBonusGetActive() {
            if (GameManager.Instance.State == GameState.Playing && TakeAimManager.Instance.State == TakeAimManager.InputState.TakeAim) {
                if (GetAllow(GetBonusCost(_displayedBonuse)) >= 1f) {
                    if (!BonusManager.Instance.ActiveBonuses.Contains(_displayedBonuse)) {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }

        private void SetImageFillAmount() => _skillImage.fillAmount = GetAllow(GetBonusCost(_displayedBonuse));

        private float GetAllow(int cost) => (float)CurrencyManager.ScoresToBuyBonuses / (float)cost;
        private int GetBonusCost(Bonuses bonuse) => (int)bonuse * BonusManager.Instance.CostMultiplier;

        private void OnEnable() {
            CurrencyManager.OnBalanceChange += SetImageFillAmount;
            InitSelf();
        }
        private void OnDisable() {
            CurrencyManager.OnBalanceChange -= SetImageFillAmount;
        }



#if UNITY_EDITOR
        private void OnValidate() {
            var go = transform.GetChild(1);
            _skillButton = _skillButton != null ? _skillButton : go.GetComponent<Button>();
            _skillImage = _skillImage != null ? _skillImage : go.GetComponent<Image>();
            _skillImage.type = Image.Type.Filled;
            _skillImage.fillMethod = Image.FillMethod.Radial360;
            _bgImage = _bgImage != null ? _bgImage : transform.GetChild(0).gameObject;

            
        }
        [ContextMenu("addRef")] 
        private void addRef() {
            _hint = GameObject.Find("SkillHint");
        }
#endif
    }
}