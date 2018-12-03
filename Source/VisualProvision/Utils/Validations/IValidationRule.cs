namespace VisualProvision.Utils.Validations
{
    public interface IValidationRule<T>
    {
        string ValidationMessage { get; set; }

        bool Check(T value);
    }
}
