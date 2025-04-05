public static class ApplicationContext
{
    public enum Slide
    {
        Login,
        Menu,
        CharacterSelection,
        Level,
        Loading
    }

    public static Slide InitSlide { get; set; } = Slide.Login;
}