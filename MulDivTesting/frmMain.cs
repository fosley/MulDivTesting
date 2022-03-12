using System;
using System.Security.Cryptography;
using System.Windows.Forms;


namespace MulDivTesting
{
    /// <summary>
    /// This form does testing on a variety of MulDiv scaling methods.
    /// </summary>
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            MyRandom rand = new MyRandom(); // I set this to 144789756456 for the times in my SO post.
            Int64 numberIterations = 1000000;
            double ns;
            txtOutput.Text = "";
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

            Int64[] randomInts = new Int64[1000000];
            Int64 scaleIntN = 15873012;
            Int64 scaleIntD = Int64.MaxValue;
            Int64 maxInt = Int64.MaxValue;
            Int64 minInt = 0;

            UInt64[] randomUInts = new UInt64[1000000];
            UInt64 scaleUIntN = 15873012;
            UInt64 scaleUIntD = UInt64.MaxValue;
            UInt64 maxUInt = UInt64.MaxValue - 1;
            UInt64 minUInt = 0;

            // Generate lists.
            try
            {
                // Generate lists of a million random numbers.
                // Do it here so it doesn't affect timing information later.
                watch.Start();
                for (int i = 0; i < numberIterations; i++)
                {
                    UInt64 temp = rand.NextUInt64();
                    randomInts[i] = MyRandom.GetRange(temp, minInt, maxInt);
                }
                watch.Stop();
                ShowResultMessage(watch.ElapsedMilliseconds, numberIterations, "PRNG Int64s");
                if (chkStats.Checked) ShowStatsMessage(ref randomInts); // Show stats if desired.

                watch.Restart();
                for (int i = 0; i < numberIterations; i++)
                {
                    UInt64 temp = rand.NextUInt64();
                    randomUInts[i] = MyRandom.GetRangeU(temp, minUInt, maxUInt);
                }
                ShowResultMessage(watch.ElapsedMilliseconds, numberIterations, "PRNG UInt64s");
                if (chkStats.Checked) ShowStatsMessageU(ref randomUInts);
            }
            catch (Exception ex)
            {
                watch.Stop();
                ShowErrorMessage("generating PRNG lists", ex.Message);

                // If we get an exception here, abort since the lists aren't valid.
                return;
            }

            // Test Int64 using doubles with 53-bit precision.
            try
            {
                Int64[] scaledInts = new Int64[numberIterations];
                watch.Restart();
                for (int i = 0; i < numberIterations; i++)
                {
                    scaledInts[i] = (Int64)(randomInts[i] * (double)scaleIntN / scaleIntD);
                }
                watch.Stop();
                ShowResultMessage(watch.ElapsedMilliseconds, numberIterations, "double Int64 scaling");
                if (chkStats.Checked) ShowStatsMessage(ref scaledInts);
            }
            catch (Exception ex)
            {
                watch.Stop();
                ShowErrorMessage("testing UInt64 scaling XX", ex.Message);
            }

            // Test UInt64 using doubles with 53-bit precision.
            try
            {
                UInt64[] scaledUInts = new UInt64[numberIterations];
                watch.Restart();
                for (int i = 0; i < numberIterations; i++)
                {
                    scaledUInts[i] = (UInt64)(randomUInts[i] * (double)scaleUIntN / scaleUIntD);
                }
                watch.Stop();
                ShowResultMessage(watch.ElapsedMilliseconds, numberIterations, "double UInt64 scaling");
                if (chkStats.Checked) ShowStatsMessageU(ref scaledUInts);
            }
            catch (Exception ex)
            {
                watch.Stop();
                ShowErrorMessage("testing UInt64 scaling XX", ex.Message);
            }

            // Test Int64 using my hack to AProgrammer's code.
            try
            {
                Int64[] scaledInts = new Int64[numberIterations];
                watch.Restart();
                for (int i = 0; i < numberIterations; i++)
                {
                    scaledInts[i] = MyRandom.MulDiv64(randomInts[i], scaleIntN, scaleIntD);
                }
                watch.Stop();
                ShowResultMessage(watch.ElapsedMilliseconds, numberIterations, "MichaelS Int64 scaling");
                if (chkStats.Checked) ShowStatsMessage(ref scaledInts);
            }
            catch (Exception ex)
            {
                watch.Stop();
                ShowErrorMessage("testing Int64 scaling 00", ex.Message);
            }
            
            // Test UInt64 using AProgrammer's code.
            try
            {
                UInt64[] scaledUInts = new UInt64[numberIterations];
                watch.Restart();
                for (int i = 0; i < numberIterations; i++)
                {
                    scaledUInts[i] = MyRandom.MulDiv64U(randomUInts[i], scaleUIntN, scaleUIntD);
                }
                watch.Stop();
                ShowResultMessage(watch.ElapsedMilliseconds, numberIterations, "MichaelS UInt64 scaling");
                if (chkStats.Checked) ShowStatsMessageU(ref scaledUInts);
            }
            catch (Exception ex)
            {
                watch.Stop();
                ShowErrorMessage("testing UInt64 scaling 00", ex.Message);
            }

            // Test Int64 using double128.
            try
            {
                Int64[] scaledInts = new Int64[numberIterations];
                watch.Restart();
                double128ProjCli.Double128 int128;
                for (int i = 0; i < numberIterations; i++)
                {
                    int128 = new double128ProjCli.Double128(randomInts[i]);
                    int128 = int128 * scaleIntN;
                    int128 = int128 / scaleIntD;
                    scaledInts[i] = (Int64)int128;
                }
                watch.Stop();
                ShowResultMessage(watch.ElapsedMilliseconds, numberIterations, "double128 Int64 scaling");
                if (chkStats.Checked) ShowStatsMessage(ref scaledInts);
            }
            catch (Exception ex)
            {
                watch.Stop();
                ShowErrorMessage("testing Int64 scaling 00", ex.Message);
            }

            // Test UInt64 using double128.
            try
            {
                UInt64[] scaledUInts = new UInt64[numberIterations];
                watch.Restart();
                double128ProjCli.Double128 int128;
                for (int i = 0; i < numberIterations; i++)
                {
                    int128 = new double128ProjCli.Double128(randomInts[i]);
                    int128 = int128 * scaleIntN;
                    int128 = int128 / scaleIntD;
                    scaledUInts[i] = (UInt64)int128;
                }
                watch.Stop();
                ShowResultMessage(watch.ElapsedMilliseconds, numberIterations, "double128 UInt64 scaling");
                if (chkStats.Checked) ShowStatsMessageU(ref scaledUInts);
            }
            catch (Exception ex)
            {
                watch.Stop();
                ShowErrorMessage("testing Int64 scaling 00", ex.Message);
            }

            // Test Int64 using Int128.
            try
            {
                Int64[] scaledInts = new Int64[numberIterations];
                watch.Restart();
                for (int i = 0; i < numberIterations; i++)
                {
                    scaledInts[i] = (Int64)(randomInts[i] * (Int128)scaleIntN / scaleIntD);
                }
                watch.Stop();
                ShowResultMessage(watch.ElapsedMilliseconds, numberIterations, "Int128 Int64 scaling");
                if (chkStats.Checked) ShowStatsMessage(ref scaledInts);
            }
            catch (Exception ex)
            {
                watch.Stop();
                ShowErrorMessage("testing Int64 scaling 00", ex.Message);
            }

            // Test UInt64 using UInt128.
            try
            {
                UInt64[] scaledUInts = new UInt64[numberIterations];
                watch.Restart();
                for (int i = 0; i < numberIterations; i++)
                {
                    scaledUInts[i] = (UInt64)(randomUInts[i] * (UInt128)scaleUIntN / scaleUIntD);
                }
                watch.Stop();
                ShowResultMessage(watch.ElapsedMilliseconds, numberIterations, "UInt128 UInt64 scaling");
                if (chkStats.Checked) ShowStatsMessageU(ref scaledUInts);
            }
            catch (Exception ex)
            {
                watch.Stop();
                ShowErrorMessage("testing Int64 scaling 00", ex.Message);
            }

        }

        private void ShowResultMessage(Int64 milliseconds, Int64 numberIterations, string resultName)
        {
            const Int64 nanosecondsPerMillisecond = 1000000;
            Int64 ns = milliseconds * nanosecondsPerMillisecond / numberIterations;
            txtOutput.Text += String.Format("{0} took {1} ms ({2} ns average).\r\n", resultName, milliseconds, ns);
        }

        private void ShowStatsMessage(ref Int64[] intList)
        {
            Int64 min = Int64.MaxValue;
            Int64 max = Int64.MinValue;
            Int64 average;
            Int128 total = 0;
            for (int i = 0; i < intList.Length; i++)
            {
                total += intList[i];
                if (intList[i] < min) min = intList[i];
                if (intList[i] > max) max = intList[i];
            }
            average = (Int64)(total / (Int128)intList.Length);
            double averagePercent = ((double)average - min);
            double averagePercent2 = ((double)max - min);
            averagePercent /= averagePercent2;
            averagePercent *= 100;

            txtOutput.Text += String.Format("Min value: {0}. Max value: {1}. Average value: {2} ({3:F1}%).\r\n", min, max, average, averagePercent);
        }

        private void ShowStatsMessageU(ref UInt64[] intList)
        {
            UInt64 min = UInt64.MaxValue;
            UInt64 max = UInt64.MinValue;
            UInt64 average;
            UInt128 total = 0;
            for (int i = 0; i < intList.Length; i++)
            {
                total += intList[i];
                if (intList[i] < min) min = intList[i];
                if (intList[i] > max) max = intList[i];
            }
            average = (UInt64)(total / (UInt128)intList.Length);
            double averagePercent = ((double)average - min) / ((double)max - min) * 100;

            txtOutput.Text += String.Format("Min value: {0}. Max value: {1}. Average value: {2} ({3:F1}%).\r\n", min, max, average, averagePercent);
        }

        private void ShowErrorMessage(string testName, string errorMessage)
        { 
            txtOutput.Text += String.Format("An exception occurred {0}.\r\n" +
                    "Exception message:\r\n{1}\r\n", testName, errorMessage);
        }

    }


    /// <summary>
    /// A random number generator using xoshiro256** algorithms.
    /// Based on code from GitHub: https://gist.github.com/i-e-b/a585fc2b9cea1e3d6221451529597145
    /// Itself from a Wikipedia description of the original C code: https://prng.di.unimi.it/xoshiro256starstar.c
    /// </summary>
    public class MyRandom
    {

        private UInt64[] state;  /// The current state of the generator, from which new bytes are generated.


        /// <summary>
        /// Seed the instance with a cryptographically-secure seed from the operating system for hard-to-guess sequences of pseudo-random numbers.
        /// </summary>
        public MyRandom()
        {
            // Use the cryptography class to generate a good, random seed (ultimately from the OS's implementation of hardware seeding).
            RandomNumberGenerator rand = RandomNumberGenerator.Create();

            // Pull the next 64 bits (8 bytes) into an array.
            byte[] randBytes = new byte[8];
            rand.GetBytes(randBytes, 0, 8);

            // Convert the 64 bits into a single 64-bit unsigned int and return it.
            UInt64 seed = BitConverter.ToUInt64(randBytes, 0);

            // Initialize state using the generated seed. 
            xorshift256_init(seed);
        }


        /// <summary>
        /// Seed the class with a known seed to reproduce previous sequences of pseudo-random numbers.
        /// </summary>
        /// <param name="seed">A 64-bit seed with which to seed this instance of the class.</param>
        public MyRandom(UInt64 seed)
        {
            // Initialize state using the provided value. 
            xorshift256_init(seed);
        }


        /// <summary>
        /// Helper function for xorshift256_init, mixes up seed values to get a pseudo-random starting state.
        /// </summary>
        /// <param name="partialstate">The state portion to splitmix.</param>
        /// <returns>The splitmixed state portion.</returns>
        private UInt64 splitmix64(UInt64 partialstate)
        {
            partialstate = partialstate + 0x9E3779B97f4A7C15;
            partialstate = (partialstate ^ (partialstate >> 30)) * 0xBF58476D1CE4E5B9;
            partialstate = (partialstate ^ (partialstate >> 27)) * 0x94D049BB133111EB;
            return partialstate ^ (partialstate >> 31);
        }


        /// <summary>
        /// Initialize the generator's state with the given seed value.
        /// </summary>
        /// <param name="seed">A 64-bit seed with which to seed this instance of the class.</param>
        private void xorshift256_init(UInt64 seed)
        {
            UInt64[] result = new UInt64[4];
            result[0] = splitmix64(seed);
            result[1] = splitmix64(result[0]);
            result[2] = splitmix64(result[1]);  // Was result[2] = splitmix64(result[2]), which seemed wrong.
            result[3] = splitmix64(result[2]);  // Was result[3] = splitmix64(result[3]), which again seemed wrong.
            this.state = result;
        }


        /// <summary>
        /// Helper function for xoshiro256p, rotates some bits around.
        /// </summary>
        /// <param name="x">The state portion being modified.</param>
        /// <param name="k">The number of bits to shift around.</param>
        /// <returns>The modified state portion.</returns>
        private UInt64 rotl64(UInt64 x, int k)
        {
            return (x << k) | (x >> (64 - k));
        }


        /// <summary>
        /// Generates the next 64-bit integer in the sequence, then updates the state to be ready for another call.
        /// </summary>
        /// <returns>The next 64-bit integer in the sequence.</returns>
        public UInt64 NextUInt64()
        {
            UInt64 result = rotl64(this.state[1] * 5, 7) * 9;
            UInt64 t = this.state[1] << 17;

            this.state[2] ^= this.state[0];
            this.state[3] ^= this.state[1];
            this.state[1] ^= this.state[2];
            this.state[0] ^= this.state[3];

            this.state[2] ^= t;
            this.state[3] = rotl64(this.state[3], 45);

            return result;
        }

        /// <summary>
        /// Get the next value in the sequence as a 64-bit signed integer.
        /// </summary>
        /// <returns>The next 64-bit signed integer in the sequence.</returns>
        public Int64 NextInt64()
        {
            // Get a UInt64, decompose to bytes, then recompose to an Int64.
            return BitConverter.ToInt64(BitConverter.GetBytes(NextUInt64()), 0);
        }

        /// <summary>
        /// Returns a random signed integer between Min and Max, inclusive.
        /// Returns min if min is Int64.MinValue and max is Int64.MaxValue.
        /// </summary>
        /// <param name="min">The minimum value that may be returned.</param>
        /// <param name="max">The maximum value that may be returned.</param>
        /// <returns>The random value selected by the Fates for your application's immediate needs. Or their fickle whims.</returns>
        public static Int64 GetRange(UInt64 randInt, Int64 min, Int64 max)
        {
            // Swap inputs if they're in the wrong order.
            if (min > max)
            {
                Int64 Temp = min;
                min = max;
                max = Temp;
            }

            // Fraction randInt/MaxValue needs to be strictly less than 1.
            if (randInt == UInt64.MaxValue) randInt = 0;

            // Get the difference between min and max values.
            UInt64 diff = (UInt64)(max - min) + 1;

            // Scale randInt from the range 0, MaxInt to the range 0, diff.
            randInt = MulDiv64U(diff, randInt, UInt64.MaxValue);

            // Convert to signed Int64.
            UInt64 randRem = randInt % 2;
            randInt /= 2;
            Int64 result = min + (Int64)randInt + (Int64)randInt + (Int64)randRem;

            // Finished.
            return result;

        }

        /// <summary>
        /// Returns a random unsigned integer between Min and Max, inclusive.
        /// (max - min) must be less than UInt64.MaxValue.
        /// </summary>
        /// <param name="min">The minimum value that may be returned.</param>
        /// <param name="max">The maximum value that may be returned.</param>
        /// <returns>The random value selected by the Fates for your application's immediate needs. Or their fickle whims.</returns>
        public static UInt64 GetRangeU(UInt64 randInt, UInt64 min, UInt64 max)
        {
            // Swap inputs if they're in the wrong order.
            if (min > max)
            {
                UInt64 Temp = min;
                min = max;
                max = Temp;
            }

            // Fraction randInt/MaxValue needs to be strictly less than 1.
            if (randInt == UInt64.MaxValue) randInt = 0;

            // Get the difference between min and max values.
            UInt64 diff = max - min + 1;

            // Scale randInt from the range 0, maxInt to the range 0, diff.
            randInt = MulDiv64U(diff, randInt, UInt64.MaxValue);

            // Add the minimum value and return the result.
            return randInt;// randInt + min;
        }

        public static Int64 MulDiv64(Int64 value, Int64 multiplier, Int64 divisor)
        {
            // Get the signs then convert to positive values.
            bool isPositive = true;
            if (value < 0) isPositive = !isPositive;
            UInt64 val = (UInt64)Math.Abs(value);
            if (multiplier < 0) isPositive = !isPositive;
            UInt64 mult = (UInt64)Math.Abs(multiplier);
            if (divisor < 0) isPositive = !isPositive;
            UInt64 div = (UInt64)Math.Abs(divisor);

            // Scale diff down.
            UInt64 scaledVal = MulDiv64U(val, mult, div);

            // Convert to signed Int64.
            Int64 result = (Int64)scaledVal;
            if (!isPositive) result *= -1;

            // Finished.
            return result;
        }

        /// <summary>
        /// Returns an accurate, 64-bit result from value * multiplier / divisor without overflow.
        /// From https://stackoverflow.com/a/8757419/5313933
        /// </summary>
        /// <param name="value">The starting value.</param>
        /// <param name="multiplier">The number to multiply by.</param>
        /// <param name="divisor">The number to divide by.</param>
        /// <returns>The result of value * multiplier / divisor.</returns>
        public static UInt64 MulDiv64U(UInt64 value, UInt64 multiplier, UInt64 divisor)
        {
            UInt64 baseVal = 1UL << 32;
            UInt64 maxdiv = (baseVal - 1) * baseVal + (baseVal - 1);

            // First get the easy thing
            UInt64 res = (value / divisor) * multiplier + (value % divisor) * (multiplier / divisor);
            value %= divisor;
            multiplier %= divisor;
            // Are we done?
            if (value == 0 || multiplier == 0)
                return res;
            // Is it easy to compute what remain to be added?
            if (divisor < baseVal)
                return res + (value * multiplier / divisor);
            // Now 0 < a < c, 0 < b < c, c >= 1ULL
            // Normalize
            UInt64 norm = maxdiv / divisor;
            divisor *= norm;
            value *= norm;
            // split into 2 digits
            UInt64 ah = value / baseVal, al = value % baseVal;
            UInt64 bh = multiplier / baseVal, bl = multiplier % baseVal;
            UInt64 ch = divisor / baseVal, cl = divisor % baseVal;
            // compute the product
            UInt64 p0 = al * bl;
            UInt64 p1 = p0 / baseVal + al * bh;
            p0 %= baseVal;
            UInt64 p2 = p1 / baseVal + ah * bh;
            p1 = (p1 % baseVal) + ah * bl;
            p2 += p1 / baseVal;
            p1 %= baseVal;
            // p2 holds 2 digits, p1 and p0 one

            // first digit is easy, not null only in case of overflow
            UInt64 q2 = p2 / divisor;
            p2 = p2 % divisor;

            // second digit, estimate
            UInt64 q1 = p2 / ch;
            // and now adjust
            UInt64 rhat = p2 % ch;
            // the loop can be unrolled, it will be executed at most twice for
            // even baseVals -- three times for odd one -- due to the normalisation above
            while (q1 >= baseVal || (rhat < baseVal && q1 * cl > rhat * baseVal + p1))
            {
                q1--;
                rhat += ch;
            }
            // subtract 
            p1 = ((p2 % baseVal) * baseVal + p1) - q1 * cl;
            p2 = (p2 / baseVal * baseVal + p1 / baseVal) - q1 * ch;
            p1 = p1 % baseVal + (p2 % baseVal) * baseVal;

            // now p1 hold 2 digits, p0 one and p2 is to be ignored
            UInt64 q0 = p1 / ch;
            rhat = p1 % ch;
            while (q0 >= baseVal || (rhat < baseVal && q0 * cl > rhat * baseVal + p0))
            {
                q0--;
                rhat += ch;
            }
            // we don't need to do the subtraction (needed only to get the remainder,
            // in which case we have to divide it by norm)
            return res + q0 + q1 * baseVal; // + q2 *baseVal*baseVal
        }


    }
}

