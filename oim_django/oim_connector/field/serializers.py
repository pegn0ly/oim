from django.db.models import fields
from rest_framework import serializers
from .models import FieldProps, ObstacleMapTemplate

class FieldInfoSerializer(serializers.ModelSerializer):
    class Meta:
        model = FieldProps
        fields = ['id', 'BaseX', 'BaseY', 'Width', 'Height']

    def create(self, validated_data):
        return super().create(validated_data)

class ObstacleMapTemplateSerializer(serializers.ModelSerializer):
    class Meta:
        model = ObstacleMapTemplate
        fields = ['obstacle_map']