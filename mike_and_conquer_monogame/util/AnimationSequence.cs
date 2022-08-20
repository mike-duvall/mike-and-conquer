

using System.Collections.Generic;

namespace mike_and_conquer_monogame.util
{
    public class AnimationSequence
    {

        private List<int> frames;
        private int frameSwitchTimer;
        private int frameSwitchThreshold;
        private int currentAnimationFrameIndex;
//        private bool animate;


        public AnimationSequence(int frameSwitchThreshold)
        {
            this.frameSwitchTimer = 0;
            this.frameSwitchThreshold = frameSwitchThreshold;
            this.frames = new List<int>();
//            animate = true;
        }

        //public void SetAnimate(bool animateFlag)
        //{
        //    this.animate = animateFlag;
        //}

        public void AddFrame(int frame)
        {
            frames.Add(frame);
        }

        public void Update()
        {
            //if(!animate)
            //{
            //    return;
            //}

            if (frameSwitchTimer > frameSwitchThreshold)
            {
                frameSwitchTimer = 0;
                currentAnimationFrameIndex++;
                if (currentAnimationFrameIndex >= frames.Count - 1)
                {
                    currentAnimationFrameIndex = 0;
                }
            }
            else
            {
                frameSwitchTimer++;
            }

        }


        public int GetCurrentFrame()
        {
//            MikeAndConquerGame.instance.log.Information("GetCurrentFrame() currentAnimationFrameIndex={0}", currentAnimationFrameIndex);
            return frames[currentAnimationFrameIndex];
        }

        public void SetCurrentFrameIndex(int currentAnimationFrame)
        {
            this.currentAnimationFrameIndex = currentAnimationFrame;
        }


    }
}
