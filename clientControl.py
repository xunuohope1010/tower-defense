import socket


TCP_IP = '127.0.0.1'
TCP_PORT = 8080
BUFFER_SIZE = 1024
send_data = input("Please input anything")

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect((TCP_IP, TCP_PORT))
s.send(send_data.encode('utf8'))
data = s.recv(BUFFER_SIZE)
# s.close()

print("received data:", data.decode('utf8'))
