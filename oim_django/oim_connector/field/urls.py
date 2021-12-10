from django.urls import path
from . import views

urlpatterns = [
    path("random/", views.FieldRandom.as_view(), name="random_field"),
    #path("get_obstacle/", views.ObstacleTemplateGetter.as_view(), name="obstacle_getter")
]