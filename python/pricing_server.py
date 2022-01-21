import grpc
from concurrent import futures
import time
import os

import price_pb2_grpc as pb2_grpc
import price_pb2 as pb2


class PricingService(pb2_grpc.PricerServicer):

    def __init__(self, *args, **kwargs):
        pass

    def GetPrice(self, request, context):
        print(request)
        if "dmc" in request.manufacturerName:
            result = {'price': 50000, 'currencyCode': 'USD'}
        elif "brown" in request.color:
            result = {'price': 200, 'currencyCode': 'GBP'}
        else:
            result = {'price': 12345, 'currencyCode': 'EUR'}

        return pb2.PriceReply(**result)


def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    pb2_grpc.add_PricerServicer_to_server(PricingService(), server)

    if os.path.isdir("D:/Dropbox/workshop.ursatile.com"):
        with open('D:/Dropbox/workshop.ursatile.com/workshop.ursatile.com.key', 'rb') as f:
            private_key = f.read()
        with open('D:/Dropbox/workshop.ursatile.com/workshop.ursatile.com.crt', 'rb') as f:
            certificate_chain = f.read()
        server_credentials = grpc.ssl_server_credentials(
            ((private_key, certificate_chain,),))

    server.add_secure_port('[::]:5003', server_credentials)

    server.start()
    print("Autobarn Python pricing server is running...")
    server.wait_for_termination()


if __name__ == '__main__':
    serve()
