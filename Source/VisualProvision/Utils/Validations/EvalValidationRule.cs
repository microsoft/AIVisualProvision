using System;

namespace VisualProvision.Utils.Validations
{
    public class EvalValidationRule<T> : IValidationRule<T>
    {
        private readonly Func<T, bool> predicate;

        public EvalValidationRule(Func<T, bool> predicate, string validationMessage)
        {
            this.predicate = predicate;
            this.ValidationMessage = validationMessage;
        }

        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            return this.predicate.Invoke(value);
        }
    }
}
