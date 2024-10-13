using Character;
using System.Collections.Generic;
using TakeAim;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game_Bonuses {
    public enum Bonuses {
        Default = 0,
        ChancheBallRandolmy = 1,
        AddForceDownToTakeAimBall = 2,
        AddForceUpToAnyBallInBox = 3,
        UniversalBall = 4,
        Ligthness = 5
    }

    public class BonusManager : MonoBehaviour {
        public static BonusManager Instance { get; private set; }
        public List<Bonuses> ActiveBonuses { get; private set; }
        public Bonuses RunningBonus { get; set; }
        public int CostMultiplier { get => _costMultiplier; private set => _costMultiplier = value; }
        [SerializeField] private int _costMultiplier;

        [SerializeField] private Volume _volume;
        [SerializeField] private float _thrustDown, _thrustUp;

        private void Awake() => Instance = this;
        private void Start() => ActiveBonuses = new List<Bonuses>();
        public void RemoveBonuseFromActiveBonusesList(Bonuses bonus) => ActiveBonuses.Remove(bonus);

        public void SetBonusState(Bonuses bonuse, CallbackVoid call = null) {
            RunningBonus = bonuse;
            GameManager.Instance.ChancheGameState(GameState.BonusFreese);
            ActiveBonuses.Add(bonuse);
            switch (bonuse) {
                case Bonuses.ChancheBallRandolmy:
                    SetChancheBallRandolmy(call);
                    break;
                case Bonuses.AddForceDownToTakeAimBall:
                    SetAddForceDownToTakeAimBall(call);
                    break;
                case Bonuses.AddForceUpToAnyBallInBox:
                    BonusStateGrapfic.Instance.EnableBG();
                    BallSelection.OnForceUpBonusCompleted += () => {
                        call?.Invoke();
                        BonusStateGrapfic.Instance.DisableBG();
                        BallSelection.OnForceUpBonusCompleted = null;
                    };
                    break;
                case Bonuses.UniversalBall:
                    GetUniversalBall(call);
                    break;
                case Bonuses.Ligthness:
                    BonusStateGrapfic.Instance.EnableBG();
                    SetLigthness(call);
                    break;
            }
        }

        [ContextMenu("SetLigthness")]
        public void SetLigthness(CallbackVoid call = null) {
            LithnessDisplay.Instance.Activate(true);
            LithnessDisplay.Instance.SetLigthnessIconMovement();
            _volume.enabled = true;
            LigthnessShader.OnGrapficHaveCorrespondMoment += () => {

                Lithness.OnBonusEnds += () => {

                    LigthnessShader.OnDisable += () => {
                        LithnessDisplay.Instance.Activate(false);
                        LigthnessShader.OnDisable = null;
                    };
                    LigthnessShader.Instance.Disable();
                    _volume.enabled = false;
                    BonusStateGrapfic.Instance.DisableBG();
                    LithnessDisplay.Instance.ReturnLigthnessIconRotation();
                    call?.Invoke();
                    Lithness.OnBonusEnds = null;
                };

                AudioManager.Instanse.PlaySound(AudioManager.Instanse.StrikeClip);
                Lithness.Instance.CastLithness();
                LigthnessShader.OnGrapficHaveCorrespondMoment = null;
            };
        }

        // Display, MaxScores, Fuctionality
        [ContextMenu("SetChancheBallRandolmy")]
        public void SetChancheBallRandolmy(CallbackVoid call = null) {
            if (TakeAimManager.Instance.State == TakeAimManager.InputState.TakeAim) {
                TakeAimManager.Instance.ChangeBall();
                call?.Invoke();
            }
        }

        [ContextMenu("SetAddForceDownToTakeAimBall")]
        public void SetAddForceDownToTakeAimBall(CallbackVoid call = null) {
            TakeAimManager.Instance.ForceToBallOnDrag = () => {
                TakeAimManager.Instance.BallOnDrag.RB.AddForce((-transform.up) * _thrustDown, ForceMode2D.Impulse);
                TakeAimManager.Instance.ForceToBallOnDrag = null;
            };
            call?.Invoke();
        }

        public void SetAddForceUpToAnyBallInBox(Ball ball, CallbackVoid call = null) {
            ball.RB.AddForce(transform.up * _thrustUp, ForceMode2D.Impulse);
        }

        [ContextMenu("GetUniversalBall")]
        public void GetUniversalBall(CallbackVoid call = null) {
            TakeAimManager.Instance.ChangeBall(true);
            call?.Invoke();
        }
    }
}