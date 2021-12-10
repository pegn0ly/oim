from django.urls import path
from . import views

urlpatterns = [
    path("new/", views.FightNew.as_view(), name="new_fight"),
    path("last/", views.FightLast.as_view(), name="last_fight"),
    path("update/", views.FightUpdate.as_view(), name="update_fight"),
    path("turn/new/", views.SaveNewFightState.as_view(), name="save_new_state"),
    path("turn/update/", views.UpdateSavedFightState.as_view(), name="update_state"),
]