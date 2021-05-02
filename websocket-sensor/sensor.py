from time import sleep
from websocket import *
import ssl
import random
import json

ws = create_connection("wss://localhost:5001/voltage/ws",
                       sslopt={"cert_reqs": ssl.CERT_NONE})


def start_client():
    error = random.random() / 1000
    currentVoltage = random.random() * 1000

    while ws.connected:
        readings = {"voltage": currentVoltage, "error": error}

        print(json.dumps(readings))
        ws.send(json.dumps(readings))
        sleep(0.1)

        currentVoltage += (random.random() - 0.5) * 2.0

    ws.close()


def main():
    try:
        start_client()
    except KeyboardInterrupt:
        ws.close()


main()
