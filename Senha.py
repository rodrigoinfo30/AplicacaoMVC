import random
import string

def embaralhar_senha(senha):
    """
    Embaralha os caracteres de uma senha fornecida.
    """
    # 1. Converter a senha para uma lista de caracteres
    lista_caracteres = list(senha)
    
    # 2. Embaralhar a lista de caracteres in-place
    random.shuffle(lista_caracteres)
    
    # 3. Unir os caracteres embaralhados de volta em uma string
    senha_embaralhada = "".join(lista_caracteres)
    
    return senha_embaralhada

# Exemplo de uso:
senha_original = "1234"
senha_embaralhada = embaralhar_senha(senha_original)

print(f"Senha Original:    {senha_original}")
print(f"Senha Embaralhada: {senha_embaralhada}")

# Exemplo de criação e embaralhamento de uma nova senha forte:
caracteres = string.ascii_letters + string.digits + string.punctuation
nova_senha_lista = random.choices(caracteres, k=16) # Cria uma lista de 16 caracteres aleatórios
random.shuffle(nova_senha_lista) # Embaralha a lista (garante maior aleatoriedade na ordem final)
nova_senha_str = "".join(nova_senha_lista)

print(f"\nNova Senha Forte Gerada e Embaralhada: {nova_senha_str}")
print("")
print("Senha em hexadecimal")
def senha_para_hex(senha):
    # Codifica a string para bytes usando UTF-8 e, em seguida, converte para hexadecimal
    bytes_senha = senha.encode('utf-8')
    hex_senha = bytes_senha.hex()
    return hex_senha

# Exemplo de uso:
minha_senha = "1234@365"
senha_hex = senha_para_hex(minha_senha)
senha_embaralhada = embaralhar_senha(senha_hex)
print(f"Senha original: {minha_senha}")
print(f"Senha em hexadecimal: {senha_hex}")
print(f"Senha em hexadecimal embaralhada: {senha_embaralhada}")

# Para decodificar de volta (se necessário):
bytes_originais = bytes.fromhex(senha_hex)
senha_original_novamente = bytes_originais.decode('utf-8')
print(f"Senha decodificada: {senha_original_novamente}")

print()
print("senha em base 64")
import base64

senha = "1234@"
email = "rodrigo@rodrigo.com.br"
# Codifica para Base64 (Resultado em ASCII)
senha_bytes = senha.encode('ascii')
email_bytes = email.encode('ascii')
senha_final = senha_bytes + email_bytes
base64_bytes = base64.b64encode(senha_final)
#base64_bytes = base64.b64encode(senha_final)
senha_ascii_final = base64_bytes.decode('ascii')

print(senha_ascii_final) 
# Saída: TWluaGFTZW5oYTEyMw==

print()
print("outra senha")
import hashlib

senha = "1234"
# Converte a string para bytes (ASCII/UTF-8) e gera o hash
hash_objeto = hashlib.sha256(senha.encode('ascii'))
senha_criptografada = hash_objeto.hexdigest()

print(senha_criptografada)
# Saída: Uma string hexadecimal longa e segura

