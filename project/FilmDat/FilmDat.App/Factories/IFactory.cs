using System;

namespace FilmDat.App.Factories
{
    public interface IFactory<out T>
    {
        T Create();
    }
}