from rest_framework.response import Response
from rest_framework.decorators import api_view, action, permission_classes
from rest_framework.permissions import IsAuthenticated
from rest_framework import status
from rest_framework import viewsets
from django.core.exceptions import ObjectDoesNotExist
from .models import Company
from .serializers import CompanySerializer
from django.contrib.auth.models import User


class CompanyViewSet(viewsets.ModelViewSet):

    queryset = Company.objects.all()
    serializer_class = CompanySerializer
    permission_classes = [IsAuthenticated]

    def retrieve(self, request, id=None):

        try:
            company = Company.objects.get(id=id)
        except ObjectDoesNotExist:
            return Response(status=status.HTTP_404_NOT_FOUND)

        serializer = CompanySerializer(company, many=False)
        return Response(serializer.data)

    def list(self, request):
        company = Company.objects.all()
        serializer = CompanySerializer(company, many=True)
        return Response(serializer.data)

    def create(self, request):
        serializer = CompanySerializer(data=request.data)
        if serializer.is_valid():
            serializer.save()
            return Response(serializer.data, status=status.HTTP_201_CREATED)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

    def update(self, request, id):

        try:
            company = Company.objects.get(id=id)
        except ObjectDoesNotExist:
            return Response(status=status.HTTP_404_NOT_FOUND)

        existing_fields = existing_fields = set([
            field.name for field in Company._meta.get_fields()])
        request_fields = set(request.data.keys())

        # Check if there are any fields in the request that don't exist in the model
        if request_fields and not request_fields.issubset(existing_fields):
            invalid_fields = request_fields - existing_fields
            return Response(
                {"error": f"Invalid fields: {', '.join(invalid_fields)}"},
                status=status.HTTP_400_BAD_REQUEST,
            )

        serializer = CompanySerializer(
            instance=company, data=request.data, partial=True)

        if serializer.is_valid():
            serializer.save()
            return Response(serializer.data)

        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

    def destroy(self, request, id):

        try:
            company = Company.objects.get(id=id)
        except ObjectDoesNotExist:
            return Response(status=status.HTTP_404_NOT_FOUND)

        company.delete()
        return Response("Company Deleted Successfully", status=status.HTTP_204_NO_CONTENT)

    @action(detail=False, methods=['GET'], url_path='byUser', url_name='company-by-user')
    def getCompanyByUser(self, request, id=None):
        try:
            # Check if the user exists
            user = User.objects.get(pk=id)
        except ObjectDoesNotExist:
            return Response(status=status.HTTP_404_NOT_FOUND)

        companies = Company.objects.filter(user=user)

        # Check if the user has any companies
        if not companies:
            return Response(status=status.HTTP_404_NOT_FOUND)

        serializer = CompanySerializer(companies, many=True)
        return Response(serializer.data)
