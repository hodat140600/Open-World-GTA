namespace _GAME._Scripts
{
    public class AnimatorSetInt : AnimatorSetValue<int>
    {
        FisherYatesRandom random = new FisherYatesRandom();
        [HelpBox("Random Value between Default Value and Max Value")]
        public bool randomEnter;
        [LeoHideInInspector("randomEnter")]
        public int maxEnterValue;
        public bool randomExit;
        [LeoHideInInspector("randomExit")]
        public int maxExitValue;

        protected override int GetEnterValue()
        {
            return randomEnter ? random.Range(base.GetEnterValue(), maxEnterValue) : base.GetEnterValue();
        }
        protected override int GetExitValue()
        {
            return randomExit ? random.Range(base.GetExitValue(), maxExitValue) : base.GetExitValue();
        }
    }
}