public interface IController
    {
        PlayerController Controller { get; set; }
        
        void Left();
        void Right();

        void Up();
        void Down();
    }
