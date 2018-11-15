# TrivialBDD

A poor-man's BDD framework sans Gherkhin.

I'm not sure if I want to do anything with this yet, but it's a simple idea to help break up method calls in test cases to aid readability. Obviously method calls will need to be written to return `this`.

e.g.

```
    public class MyTests
    {
        public void MyFirstTest()
        {
            this
                .Given().CreateUser("Steve")
                .And().EnableUser("Steve")
                .Then().CanLogin("Steve");
        }
        
        private MyTests CreateUser(string name)
        {
            Users.Create(name);
            return this;
        }
        
        private MyTests EnableUser(string name)
        {
            Users.Enable(name);
            return this;
        }

        private MyTests CanLogin(string name)
        {
            Assert.That(LoginService.Login(name), Is.True);
            return this;
        }
    }
```
