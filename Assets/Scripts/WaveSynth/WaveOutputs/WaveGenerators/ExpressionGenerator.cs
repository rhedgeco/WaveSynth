using System;
using org.mariuszgromada.math.mxparser;
using UnityEngine;

namespace WaveSynth.WaveOutputs.WaveGenerators
{
    public class ExpressionGenerator : FunctionGenerator
    {
        [Header("Expression")]
        [SerializeField] private string expression = "sin(2 * pi * x)";
        
        private float[] _compiled;
        private Argument _argument;
        private Expression _expression;

        private void Start()
        {
            Compile();
        }

        protected override float SampleFunction(double phase)
        {
            return _compiled[(int) (_compiled.Length * phase)];
        }

        public void Compile()
        {
            _argument = new Argument("x = 0");
            _expression = new Expression(expression, _argument);
            _compiled = new float[WaveSettings.SampleRate];

            float value;
            for (int i = 0; i < _compiled.Length; i++)
            {
                _argument.setArgumentValue((double) i / _compiled.Length);
                value = Convert.ToSingle(_expression.calculate());
                _compiled[i] = value;
            }
        }
    }
}