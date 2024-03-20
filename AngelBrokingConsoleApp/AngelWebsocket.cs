using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace AngelBroking
{
    public class WebSocketWrapper
    {
        private WebSocket _ws;
        private int counter = 0;

        public WebSocketWrapper()
        {
            _ws = new WebSocket();

            // Attach event handlers
            _ws.OnConnect += _OnConnect;
            _ws.OnClose += _OnClose;
            _ws.OnData += _OnData;
            _ws.OnError += _OnError;
        }

        // Event handlers
        private void _OnConnect()
        {
            Console.WriteLine("WebSocket connected.");
            // Add your logic here
            
        }

        private void _OnClose()
        {
            Console.WriteLine("WebSocket closed.");
            // Add your logic here
        }

        private void _OnError(string Message)
        {
            Console.WriteLine($"Error occurred: {Message}");
            // Add your logic here
        }

        

        // Wrapper methods
        public void Connect(string Url, Dictionary<string, string> headers = null)
        {
            _ws.Connect(Url, headers);
           
        }

        public void Send(string Message)
        {
            
            _ws.Send(Message);
        }

        public void Close()
        {
            _ws.Close();
        }

        private void _OnData(byte[] Data, int Count, string MessageType)
        {
            //Console.WriteLine($"Data received: {Encoding.UTF8.GetString(Data, 0, Count)}");
            var dataBytes = Data.ToArray();
            //Parse the binary data
            int subscriptionMode = (sbyte)Data.AsSpan()[0];
            int exchangeType = (sbyte)Data.AsSpan()[1];
            int div = exchangeType == 13 ? 10000000 : 100;
            int tokenLength = Data.AsSpan().Slice(2, 25).IndexOf((byte)0);
            if (tokenLength < 0) tokenLength = 25;
            string token = Encoding.UTF8.GetString(dataBytes, 2, 25);


            long exchangeTimestamp = BitConverter.ToInt64(dataBytes, 35);
            double lastTradedPrice = (double)BitConverter.ToInt32(dataBytes, 43) / div;
            double Lasttradedquantity = BitConverter.ToInt64(dataBytes, 51);
            double averageTradePrice = BitConverter.ToInt64(dataBytes, 59) / div;
            long VolumeTradedToday = BitConverter.ToInt64(dataBytes, 67);
            double totalBuyQuantity = BitConverter.ToDouble(dataBytes, 75);
            double totalSellQuantity = BitConverter.ToDouble(dataBytes, 83);
            long openPrice = BitConverter.ToInt64(dataBytes, 91);
            long highPrice = BitConverter.ToInt64(dataBytes, 99);
            long lowPrice = BitConverter.ToInt64(dataBytes, 107);
            long closePrice = BitConverter.ToInt64(dataBytes, 115);
            long lastTradedTimestamp = BitConverter.ToInt64(dataBytes, 123);
            long openInterest = BitConverter.ToInt64(dataBytes, 131);
            double openInterestChangePercent = BitConverter.ToDouble(dataBytes, 139);

            short buySellFlag1 = BitConverter.ToInt16(dataBytes, 147);
            long bq1 = BitConverter.ToInt64(dataBytes, 149);
            double bp1 = BitConverter.ToInt64(dataBytes, 157) / (double)div;
            long bo1 = BitConverter.ToInt64(dataBytes, 165);

            short buySellFlag2 = BitConverter.ToInt16(dataBytes, 167);
            long bq2 = BitConverter.ToInt64(dataBytes, 169);
            double bp2 = BitConverter.ToInt64(dataBytes, 177) / (double)div;
            long bo2 = BitConverter.ToInt64(dataBytes, 185);

            short buySellFlag3 = BitConverter.ToInt16(dataBytes, 187);
            long bq3 = BitConverter.ToInt64(dataBytes, 189);
            double bp3 = BitConverter.ToInt64(dataBytes, 197) / (double)div;
            long bo3 = BitConverter.ToInt64(dataBytes, 205);

            short buySellFlag4 = BitConverter.ToInt16(dataBytes, 207);
            long bq4 = BitConverter.ToInt64(dataBytes, 209);
            double bp4 = BitConverter.ToInt64(dataBytes, 217) / (double)div;
            long bo4 = BitConverter.ToInt64(dataBytes, 225);

            short buySellFlag5 = BitConverter.ToInt16(dataBytes, 227);
            long bq5 = BitConverter.ToInt64(dataBytes, 229);
            double bp5 = BitConverter.ToInt64(dataBytes, 237) / (double)div;
            long bo5 = BitConverter.ToInt64(dataBytes, 245);

            short sellSellFlag1 = BitConverter.ToInt16(dataBytes, 247);
            long sq1 = BitConverter.ToInt64(dataBytes, 249);
            double sp1 = BitConverter.ToInt64(dataBytes, 257) / (double)div;
            long so1 = BitConverter.ToInt64(dataBytes, 265);

            short sellSellFlag2 = BitConverter.ToInt16(dataBytes, 267);
            long sq2 = BitConverter.ToInt64(dataBytes, 269);
            double sp2 = BitConverter.ToInt64(dataBytes, 277) / (double)div;
            long so2 = BitConverter.ToInt64(dataBytes, 285);

            short sellSellFlag3 = BitConverter.ToInt16(dataBytes, 287);
            long sq3 = BitConverter.ToInt64(dataBytes, 289);
            double sp3 = BitConverter.ToInt64(dataBytes, 297) / (double)div;
            long so3 = BitConverter.ToInt64(dataBytes, 305);

            short sellSellFlag4 = BitConverter.ToInt16(dataBytes, 307);
            long sq4 = BitConverter.ToInt64(dataBytes, 309);
            double sp4 = BitConverter.ToInt64(dataBytes, 317) / (double)div;
            long so4 = BitConverter.ToInt64(dataBytes, 325);

            short sellSellFlag5 = BitConverter.ToInt16(dataBytes, 327);
            long sq5 = BitConverter.ToInt64(dataBytes, 329);
            double sp5 = BitConverter.ToInt64(dataBytes, 337) / (double)div;
            long so5 = BitConverter.ToInt64(dataBytes, 345);
            double lc = (double)BitConverter.ToInt64(dataBytes, 347) / div;
            double uc = (double)BitConverter.ToInt64(dataBytes, 355) / div;
            double h52 = (double)BitConverter.ToInt64(dataBytes, 363) / div;
            double l52 = (double)BitConverter.ToInt64(dataBytes, 371) / div;


            // Fill the NorenFeed object
            NorenFeed feed = new NorenFeed();

            feed.e = subscriptionMode.ToString();
            feed.tk = token;
            feed.ft = exchangeTimestamp.ToString();

            feed.lp = lastTradedPrice.ToString();
            feed.ltq = Lasttradedquantity.ToString();
            feed.ap = averageTradePrice.ToString();
            feed.v = VolumeTradedToday.ToString();
            feed.tbq = totalBuyQuantity.ToString();
            feed.tsq = totalSellQuantity.ToString();
            feed.o = openPrice.ToString();
            feed.h = highPrice.ToString();
            feed.l = lowPrice.ToString();
            feed.c = closePrice.ToString();
            feed.ltt = lastTradedTimestamp.ToString(); ;
            feed.oi = openInterest.ToString();
            feed.poi = openInterestChangePercent.ToString();


            feed.bp1 = bp1.ToString();
            feed.bq1 = bq1.ToString();
            feed.bo1 = bo1.ToString();
            
                            feed.bp2 = bp2.ToString();
                            feed.bq2 = bq2.ToString();
                            feed.bo2 = bo2.ToString();

                            feed.bp3 = bp3.ToString();
                            feed.bq3 = bq3.ToString();
                            feed.bo3 = bo3.ToString();

                            feed.bp4 = bp4.ToString();
                            feed.bq4 = bq4.ToString();
                            feed.bo4 = bo4.ToString();

                            feed.bp5 = bp5.ToString();
                            feed.bq5 = bq5.ToString();
                            feed.bo5 = bo5.ToString();

                            feed.sp1 = sp1.ToString();
                            feed.sq1 = sq1.ToString();
                            feed.so1 = so1.ToString();

                            feed.sp2 = sp2.ToString();
                            feed.sq2 = sq2.ToString();
                            feed.so2 = so2.ToString();

                            feed.sp3 = sp3.ToString();
                            feed.sq3 = sq3.ToString();
                            feed.so3 = so3.ToString();

                            feed.sp4 = sp4.ToString();
                            feed.sq4 = sq4.ToString();
                            feed.so4 = so4.ToString();

                            feed.sp5 = sp5.ToString();
                            feed.sq5 = sq5.ToString();
                            feed.so5 = so5.ToString();
                            feed.lc = lc.ToString();
                            feed.uc = uc.ToString();
                            feed.h52 = h52.ToString();
                            feed.l52 = l52.ToString();
            

            // Use reflection to get the fields
            var fields = typeof(NorenFeed).GetFields();
            if (counter > 10)
            {
                return;
            }
            Count = 0;
            // Iterate over the fields and print each one
            foreach (var field in fields)
            {
                if (Count > 7)
                {
                    break;
                }
                Console.Write($"{field.Name} : {field.GetValue(feed)} ,  ");
                Count ++;
            }
            Console.Write("\n");
            counter++;


            // Add your logic here
        }



        public class NorenFeed
        {
            public string e;
            public string tk;
            public string pp;
            public string ts;
            public string ti;
            public string ls;
            public string lp;
            public string pc;
            public string v;
            public string o;
            public string h;
            public string l;
            public string c;
            public string ap;
            public string oi;
            public string poi;
            public string toi;
            public string ltt;
            public string ltq;
            public string tbq;
            public string tsq;
            public string bq1;
            public string bq2;
            public string bq3;
            public string bq4;
            public string bq5;
            public string bp1;
            public string bp2;
            public string bp3;
            public string bp4;
            public string bp5;
            public string bo1;
            public string bo2;
            public string bo3;
            public string bo4;
            public string bo5;
            public string sq1;
            public string sq2;
            public string sq3;
            public string sq4;
            public string sq5;
            public string sp1;
            public string sp2;
            public string sp3;
            public string sp4;
            public string sp5;
            public string so1;
            public string so2;
            public string so3;
            public string so4;
            public string so5;
            public string lc;
            public string uc;
            [JsonProperty(PropertyName = "52h")]
            public string h52;
            [JsonProperty(PropertyName = "52l")]
            public string l52;
            public string ft;
        }




    }
}
