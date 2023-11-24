from django.urls import path, include
from rest_framework.routers import DefaultRouter
from .views import CompanyViewSet

router = DefaultRouter()
router.register('', CompanyViewSet)

urlpatterns = [
    path('', CompanyViewSet.as_view(
        {'get': 'list', 'post': 'create'}), name='company-list'),
    path('<int:id>/', CompanyViewSet.as_view(
        {'get': 'retrieve', 'put': 'update', 'delete': 'destroy'}), name='company-detail'),
    path('byUser/<int:id>/',
         CompanyViewSet.as_view({'get': 'getCompanyByUser'}), name='company-by-user'),
]
