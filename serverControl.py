import socket

TCP_IP = '127.0.0.1'
TCP_PORT = 8080
BUFFER_SIZE = 1024  # Normally 1024, but we want fast response

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

s.bind((TCP_IP, TCP_PORT))
s.listen(1)
print("Server start!")
conn, address = s.accept()
print('Connection address:', address)
while True:
    data = conn.recv(BUFFER_SIZE)
    print(data.decode('utf8'))
    if not data:
        break
    print("received data:", data.decode('utf8'))
    conn.send(data)  # echo
conn.close()
