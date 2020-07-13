﻿using System;
using System.Collections.Generic;
using System.Text;
using FilmDat.BL.Messages;

namespace FilmDat.BL.Services
{
    public interface IMediator
    {
        void Register<TMessage>(Action<TMessage> action)
            where TMessage : IMessage;

        void Send<TMessage>(TMessage message)
            where TMessage : IMessage;

        void UnRegister<TMessage>(Action<TMessage> action)
            where TMessage : IMessage;
    }
}