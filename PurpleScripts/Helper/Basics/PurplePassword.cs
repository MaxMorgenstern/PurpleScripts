/*
 * THIS IS A MODIFIED VERSION!!!
 * Find the original at: https://crackstation.net/hashing-security.htm
 *
 * -----
 *
 * Password Hashing With PBKDF2 (http://crackstation.net/hashing-security.htm).
 * Copyright (c) 2013, Taylor Hornby
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice,
 * this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation
 * and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */

using System.Security.Cryptography;
using System;

public class PurplePassword {

	private int SALT_BYTE_SIZE = PurpleConfig.Password.SaltByteSize;
	private int HASH_BYTE_SIZE = PurpleConfig.Password.HashByteSize;
	private int PBKDF2_ITERATIONS = PurpleConfig.Password.PBKDF2.Iterations;

	private int ITERATION_INDEX = PurpleConfig.Password.IterationIndex;
	private int SALT_INDEX = PurpleConfig.Password.SaltIndex;
	private int PBKDF2_INDEX = PurpleConfig.Password.PBKDF2.Index;

	public string CreateHash(string password)
	{
		// Generate a random salt
		RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
		byte[] salt = new byte[SALT_BYTE_SIZE];
		csprng.GetBytes(salt);

		// Hash the password and encode the parameters
		byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);
		return PBKDF2_ITERATIONS + ":" +
			Convert.ToBase64String(salt) + ":" +
				Convert.ToBase64String(hash);
	}

	public bool ValidatePassword(string password, string correctHash)
	{
		if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(correctHash)) return false;

		// Extract the parameters from the hash
		char[] delimiter = { ':' };
		string[] split = correctHash.Split(delimiter);
		int iterations = Int32.Parse(split[ITERATION_INDEX]);
		byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
		byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);

		byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
		return SlowEquals(hash, testHash);
	}

	private bool SlowEquals(byte[] a, byte[] b)
	{
		uint diff = (uint)a.Length ^ (uint)b.Length;
		for (int i = 0; i < a.Length && i < b.Length; i++)
			diff |= (uint)(a[i] ^ b[i]);
		return diff == 0;
	}

	private byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
	{
		Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
		pbkdf2.IterationCount = iterations;
		return pbkdf2.GetBytes(outputBytes);
	}
}
