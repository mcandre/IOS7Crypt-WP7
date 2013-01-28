/*
   Andrew Pennebaker
   Copyright 2005-2011 Andrew Pennebaker
*/

#include <string.h>
#include <time.h>
#include <ctype.h>
#include <stdlib.h>
#include <stdio.h>

static int xlat[];

static int XLAT_SIZE;

static void usage(char *program);

static int htoi(char x);

void encrypt(char *password, char *hash);

void decrypt(char *hash, char *password);

bool reversible(void *data);