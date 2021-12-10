from django.db.models.query import QuerySet
from django.http.request import HttpRequest
from django.shortcuts import render
from django.views.decorators.csrf import ensure_csrf_cookie, csrf_exempt
from rest_framework.fields import Field

from .models import FieldProps, ObstacleMapTemplate

from rest_framework.views import APIView
from rest_framework.authentication import TokenAuthentication
from rest_framework.response import Response
from rest_framework.permissions import IsAuthenticated
from rest_framework import serializers, status
#
from .serializers import FieldInfoSerializer, ObstacleMapTemplateSerializer

import random

class FieldRandom(APIView):
    permission_classes = (IsAuthenticated,)
    authentication_classes = (TokenAuthentication,)

    def get(self, request : HttpRequest):
        try:
            field_count = FieldProps.objects.count()
            field_info = FieldProps.objects.get(id=random.randint(0, field_count - 1))
            serializer = FieldInfoSerializer(field_info)
            print(serializer.data)
            return Response(serializer.data)
        except FieldProps.DoesNotExist:
            return Response()

# пока не юзается
class ObstacleTemplateGetter(APIView):
    permission_classes = (IsAuthenticated,)
    authentication_classes = (TokenAuthentication,)

    def get(self, request : HttpRequest):
        try:
            print("headers: " + str(request.headers))
            possible_maps : QuerySet = ObstacleMapTemplate.objects.filter(field_id=request.headers["X-Field-Id"])
            serializer = ObstacleMapTemplateSerializer(possible_maps[random.randint(0, possible_maps.count()-1)])
            print(serializer.data)
            return Response(serializer.data)
        except ObstacleMapTemplate.DoesNotExist:
            return Response()