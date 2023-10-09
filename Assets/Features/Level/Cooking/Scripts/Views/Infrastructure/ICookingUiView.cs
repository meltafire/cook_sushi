namespace Assets.Features.Level.Cooking.Scripts.Views.Infrastructure
{
    public interface ICookingUiView
    {
        public ButtonView BackButtonView { get; }
        public ButtonView DoneButtonView { get; }
        public ButtonView RevertButtonView { get; }

        public void Toggle(bool isOn);
    }
}