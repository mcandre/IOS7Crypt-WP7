/*
   Andrew Pennebaker
   Copyright 2005-2011 Andrew Pennebaker
*/

#include <string.h>
#include <time.h>
#include <ctype.h>
#include <stdlib.h>
#include <stdio.h>

static int xlat[] = {
	0x64, 0x73, 0x66, 0x64, 0x3b, 0x6b, 0x66, 0x6f,
	0x41, 0x2c, 0x2e, 0x69, 0x79, 0x65, 0x77, 0x72,
	0x6b, 0x6c, 0x64, 0x4a, 0x4b, 0x44, 0x48, 0x53,
	0x55, 0x42, 0x73, 0x67, 0x76, 0x63, 0x61, 0x36,
	0x39, 0x38, 0x33, 0x34, 0x6e, 0x63, 0x78, 0x76,
	0x39, 0x38, 0x37, 0x33, 0x32, 0x35, 0x34, 0x6b,
	0x3b, 0x66, 0x67, 0x38, 0x37
};

static int XLAT_SIZE = 53;

static void usage(char *program) {
	printf("Usage: %s [options]\n\n", program);
	printf("-e <passwords>\n");
	printf("-d <hashes>\n");
	printf("-t unit test\n");

	exit(0);
}

static int htoi(char x) {
	if(isdigit(x))
		return (int) (x - '0');
	else
		return (int) (toupper(x) - 'A' + 10);
}

void encrypt(char *password, char *hash) {
	int password_length, seed, i;

	char *temp = (char *) malloc(3);

	if (temp != NULL && password != NULL && strlen(password) > 0 && hash != NULL) {
		password_length = (int) strlen(password);

		seed = rand() % 16;

		(void) snprintf(hash, 3, "%02d", seed);

		for (i = 0; i < password_length; i++) {
			(void) snprintf(temp, 3, "%02x", ((unsigned int) password[i]) ^ xlat[(seed++) % XLAT_SIZE]);
			strcat(hash, temp);
		}
	}

	free(temp);
}

void decrypt(char *hash, char *password) {
	int seed, index, i, c;

	if (hash != NULL && strlen(hash) > 3 && password != NULL) {
		seed = htoi(hash[0]) * 10 + htoi(hash[1]);

		index = 0;

		for (i = 2; i < (int) strlen(hash); i += 2) {
			c = htoi(hash[i]) * 16 + htoi(hash[i + 1]);
			password[index++] = (char) c ^ xlat[(seed++) % XLAT_SIZE];
		}
	}
}

bool reversible(void *data) {
	int i;

	char* password = qc_args(char*, 0, sizeof(char*));

	char* hash = (char*) malloc((size_t) strlen(password) * 2 + 3);
	for (i = 0; i < strlen(password) * 2 + 3; i++) {
		hash[i] = '\0';
	}

	encrypt(password, hash);

	char* password2 = (char*) malloc((size_t) strlen(hash) / 2 * sizeof(char));
	for (i = 0; i < strlen(hash) / 2; i++) {
		password2[i] = '\0';
	}

	decrypt(hash, password2);

	int cmp = strcmp(password, password2);

	free(hash);
	free(password2);

	return cmp == 0;
}