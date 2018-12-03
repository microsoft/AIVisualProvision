using VisualProvision.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VisualProvision.Utils.Validations
{
    public class ValidatableObject<T> : BindableBase, IValidity
    {
        private readonly List<IValidationRule<T>> validations;
        private readonly List<string> errors;
        private T innerValue = default(T);
        private bool isValid;

        public event EventHandler<T> ValueChanged;

        public ValidatableObject()
        {
            isValid = true;
            errors = new List<string>();
            validations = new List<IValidationRule<T>>();
        }

        public List<IValidationRule<T>> Validations => validations;

        public List<string> Errors => errors;

        public T Value
        {
            get => innerValue;

            set
            {
                if (SetProperty(ref innerValue, value))
                {
                    NotifyValueChanged();
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return isValid;
            }

            set
            {
                isValid = value;
                OnPropertyChanged();
            }
        }

        public bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = validations
                .Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage);

            foreach (var error in errors)
            {
                Errors.Add(error);
            }

            isValid = !Errors.Any();

            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(Errors));

            return isValid;
        }

        public void ClearErrors()
        {
            Errors.Clear();
            OnPropertyChanged(nameof(Errors));
        }

        private void NotifyValueChanged()
        {
            ValueChanged?.Invoke(this, innerValue);
        }
    }
}
