namespace ResearchAtlas.Application.Abstractions.Security;

public interface ITextEncryption
{
    string Encrypt(string plaintext);

    string Decrypt(string ciphertext);
}
