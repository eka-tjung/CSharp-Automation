        [Test]
        public void ReverseSentence()
        {
            string str = "Reverse Me This Is just a test";
            string[] strs = str.Split(' ');

            StringBuilder sb = new StringBuilder();

            for(int i= strs.Length-1; i>=0; i--)
            {
                sb.Append(strs[i]);
                if (i != 0)
                {
                    sb.Append(' ');
                }
            }

            string result = sb.ToString();
            // "test a just Is This Me Reverse"
            Console.WriteLine("Result: {0}", result);
        }

        [Test]
        public void ReverseSentence2()
        {
            char[] str = "Reverse Me This Is just a test".ToCharArray();

            ReverseWord(str, 0, str.Length - 1);

            String intermediate = new String(str);

            int start = 0;
            for(int i=0; i<str.Length-1; i++)
            {
                if(str[i] == ' ')
                {
                    ReverseWord(str, start, i - 1);
                    start = i + 1;
                }
            }
            ReverseWord(str, start, str.Length - 1);

            String result = new String(str);
        }

        private string ReverseWord(char[] str, int start, int end)
        {
            if (str == null || start < 0 || end > str.Length-1) {
                return null;
            }

            while(start<end)
            {
                char temp = str[end];
                str[end] = str[start];
                str[start] = temp;
                start++;
                end--;
            }

            return str.ToString();
        }
