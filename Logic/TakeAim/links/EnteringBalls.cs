using Character;
using UnityEngine;

namespace TakeAim {
	public class EnteringBalls : MonoBehaviour {
		public static EnteringBalls Instance { get; private set; }

        private void Awake() => Instance = this;

        public BallSize ChanheBall(BallSize previousBallSize) {
            var newSize = GetEnteringBall();
            if (newSize != previousBallSize) {
                return newSize;
            }
            else {
                return ChanheBall(previousBallSize);
            }
        }

        public BallSize GetEnteringBall() {
			int rand = Random.Range(0, 12);
			if (rand == 0) {
				return (BallSize)4; // 10%
			}
            else if (rand <= 2) {
                return (BallSize)3; // 20%
            }
            else if (rand <= 7) {
                return (BallSize)2; // 30%
            }
            else {
                return (BallSize)1; // 40%
            }            
		}



#if UNITY_EDITOR
        [ContextMenu("Test")]
        private void Test() {
            print(GetEnteringBall().ToString());
        } 
#endif

    }

    //public enum BallSize {
    //    Little = 1,
    //    Tiny = 2,
    //    Smallest = 3,
    //    Small = 4,
    //    Middle = 5,
    //    Big = 6,
    //    Bigger = 7,
    //    Biggest = 8,
    //    Large = 9,
    //    Gigant = 10
    //}
}