﻿using System;
using Zoe.Exchanges;
using Zoe.Indicators;

namespace Zoe.Strategies
{
    public class RSI_Strategy
    {
        private readonly int _overbought;
        private readonly int _oversold;

        private readonly RSI _rsi = new RSI();

        public RSI_Strategy(int overbought = 70, int oversold = 30)
        {
            _overbought = overbought;
            _oversold = oversold;
        }

        private bool CollectionData => _rsi.CollectingData;

        public event EventHandler Sell;
        public event EventHandler Buy;

        public void Execute(Span<Trade> trades)
        {
            _rsi.Calculate(trades.Slice(0, _rsi._periods));

            if (_rsi.CollectingData)
                return;

            if (_rsi.Current.RSI > _overbought)
                BuySignal();
            else if (_rsi.Current.RSI < _oversold) SellSignal();
        }

        private void BuySignal()
        {
            Buy?.Invoke(this, EventArgs.Empty);
        }

        private void SellSignal()
        {
            Sell?.Invoke(this, EventArgs.Empty);
        }

        public override string ToString()
        {
            if (CollectionData) return "Collection data...";

            return _rsi.Current.RSI.ToString();
        }
    }
}