#from typing import KeysView
from django.http.request import HttpRequest
from django.shortcuts import render
#from django.views.decorators.csrf import ensure_csrf_cookie, csrf_exempt

from .models import FightProps, SavedFightCondition

from rest_framework.views import APIView
from rest_framework.authentication import TokenAuthentication
from rest_framework.response import Response
from rest_framework.permissions import IsAuthenticated
from rest_framework import serializers, status
#
from .serializers import FightInfoSerializer, SaveNewFightStateSerializer, UpdateFightStateSerializer

# регистрирует новый бой в бэкенде
class FightNew(APIView):
    permission_classes = (IsAuthenticated,)
    authentication_classes = (TokenAuthentication,)

    def post(self, request : HttpRequest):
        serializer = FightInfoSerializer(data=request.POST.dict())
        if serializer.is_valid():
            serializer.save()
            return Response(serializer.data, status=status.HTTP_201_CREATED)
        else:
            return Response(serializer.errors, status=status.HTTP_409_CONFLICT)

# возвращает в юнити последний зарегистрированный в бэкенде бой
class FightLast(APIView):
    permission_classes = (IsAuthenticated,)
    authentication_classes = (TokenAuthentication,)

    def get(self, request : HttpRequest):
        last_fight = FightProps.objects.last()
        serializer = FightInfoSerializer(last_fight)
        return Response(serializer.data, status=status.HTTP_200_OK)

# обновляет информацию о существующем бое в бэкенде
class FightUpdate(APIView):
    permission_classes = (IsAuthenticated,)
    authentication_classes = (TokenAuthentication,)    

    def post(self, request : HttpRequest):
        data = request.POST.dict()
        try:
            fight_to_update = FightProps.objects.get(Id=data["Id"])
            serializer = FightInfoSerializer(instance=fight_to_update, data=data)
            if serializer.is_valid():
                serializer.save()
                return Response(serializer.data, status=status.HTTP_200_OK)
        except Exception as e:
            return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

# сохраняет новое состояние боя в бэкенде
class SaveNewFightState(APIView):
    permission_classes = (IsAuthenticated,)
    authentication_classes = (TokenAuthentication,)   

    def post(self, request : HttpRequest):
        serializer = SaveNewFightStateSerializer(data=request.POST.dict())
        if serializer.is_valid():
            serializer.save()
            return Response(serializer.data, status=status.HTTP_201_CREATED)
        else:
            return Response(serializer.errors, status=status.HTTP_409_CONFLICT)

# обновляет информацию о сохраненном состоянии боя
class UpdateSavedFightState(APIView):
    permission_classes = (IsAuthenticated,)
    authentication_classes = (TokenAuthentication,)   

    def post(self, request : HttpRequest):
        data_to_update = request.POST.dict()
        print("data to update : " + str(data_to_update))
        fight_state_to_update = SavedFightCondition.objects.get(FightID=data_to_update["FightID"], Turn=data_to_update["Turn"])
        serializer = UpdateFightStateSerializer(instance=fight_state_to_update, data=data_to_update)
        print(str(serializer))
        if serializer.is_valid():
            print("Serializer is valid")
            serializer.save()
            return Response(serializer.data, status=status.HTTP_200_OK)
        else:
            print(serializer.errors)
            return Response(serializer.errors, status=status.HTTP_409_CONFLICT)



    
