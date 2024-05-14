using System.Text;
using Android.Text;
using Android.Util;
using Java.Lang;
using Java.Security;
using Java.Security.Spec;
using Java.IO;

namespace gorbul
{
    class _Security
    {
        private const System.String TAG = "IABUtil/Security";

        private const System.String KEY_FACTORY_ALGORITHM = "RSA";
        private const System.String SIGNATURE_ALGORITHM = "SHA1withRSA";

        /**
         * Verifies that the data was signed with the given signature, and returns the verified
         * purchase.
         * @param base64PublicKey the base64-encoded public key to use for verifying.
         * @param signedData the signed JSON System.String (signed, not encrypted)
         * @param signature the signature for the data, signed with the private key
         * @throws IOException if encoding algorithm is not supported or key specification
         * is invalid
         */
        public static bool VerifyPurchase(System.String base64PublicKey, System.String signedData, System.String signature)
        {
            if (TextUtils.IsEmpty(signedData) || TextUtils.IsEmpty(base64PublicKey)
                    || TextUtils.IsEmpty(signature))
            {
                //Purchase verification failed: missing data
                return false;
            }

            IPublicKey key = GeneratePublicKey(base64PublicKey);
            return Verify(key, signedData, signature);
        }

        /**
         * Generates a PublicKey instance from a System.String containing the Base64-encoded public key.
         *
         * @param encodedPublicKey Base64-encoded public key
         * @throws IOException if encoding algorithm is not supported or key specification
         * is invalid
         */
        public static IPublicKey GeneratePublicKey(System.String encodedPublicKey)
        {
            try
            {
                byte[] decodedKey = Base64.Decode(encodedPublicKey, Base64Flags.Default);
                KeyFactory keyFactory = KeyFactory.GetInstance(KEY_FACTORY_ALGORITHM);
                return keyFactory.GeneratePublic(new X509EncodedKeySpec(decodedKey));
            }
            catch (NoSuchAlgorithmException e)
            {
                // "RSA" is guaranteed to be available.
                throw new RuntimeException(e);
            }
            catch (InvalidKeySpecException e)
            {
                System.String msg = "Invalid key specification: " + e;
                throw new IOException(msg);
            }
        }

        /**
         * Verifies that the signature from the server matches the computed signature on the data.
         * Returns true if the data is correctly signed.
         *
         * @param publicKey public key associated with the developer account
         * @param signedData signed data from server
         * @param signature server signature
         * @return true if the data and signature match
         */
        public static bool Verify(IPublicKey publicKey, System.String signedData, System.String signature)
        {
            byte[] signatureBytes;
            try
            {
                signatureBytes = Base64.Decode(signature, Base64Flags.Default);
            }
            catch (IllegalArgumentException)
            {
                //Base64 decoding failed
                return false;
            }
            try
            {
                Signature signatureAlgorithm = Signature.GetInstance(SIGNATURE_ALGORITHM);
                signatureAlgorithm.InitVerify(publicKey);
                signatureAlgorithm.Update(Encoding.ASCII.GetBytes(signedData));
                if (!signatureAlgorithm.Verify(signatureBytes))
                {
                    //Signature verification failed
                    return false;
                }
                return true;
            }
            catch (NoSuchAlgorithmException e)
            {
                // "RSA" is guaranteed to be available
                throw new RuntimeException(e);
            }
            catch (InvalidKeyException)
            {
                //Invalid key specification
            }
            catch (SignatureException)
            {
                //Signature exception
            }
            return false;
        }

        public static bool VerifyValidSignature(System.String signedData, System.String signature)
        {
            try
            {
                // To get key go to Developer Console > Select your app > Development Tools > Services & APIs.
                const System.String base64Key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAoR4AMLYX4UWpHriYj0x/7NumxaxncoXTwsuIVEcXQ4NiLwLhk2IBQK1EZ4OLtWfZeOPeO1ZN0ioXJUduAZA23Buc5yV9Or6OB2fJK4RU2zZuPT3jC6E6Qx8yWUWZm/Y9FGy9DXpGIEFPksdu0/QJwr6CZZZH7w6N/ONqIFLLQkiQBCk/EbbhQf8pj5yjDVWWd0ViNKtlqH6SbL1CIYbHggJr7ed8d3eYhLUyUxsS188s7ac030DpvOPmGCtstDKkEs1WziHbkb5htZ1SnUKq257iHo/QnRJlqa6ZS4pl8NrgCAizKnF4PAf97cVQMYVDIClzaF9q+jfb/ovv0c4WCwIDAQAB";

                return _Security.VerifyPurchase(base64Key, signedData, signature);
            }
            catch (Java.IO.IOException)
            {
                return false;
            }
        }
    }
}