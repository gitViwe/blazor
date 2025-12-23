#!/bin/sh

# The file path where Nginx serves the static files
APPSETTINGS="/usr/share/nginx/html/appsettings.json"

echo "Replacing environment variables in $APPSETTINGS..."

# We use | as a delimiter to avoid issues with slashes in URIs
sed -i "s|<gateway-api-endpoint>|${GATEWAY_API_URI}|g" "$APPSETTINGS"
sed -i "s|<seq-ui-endpoint>|${SEQ_UI_URI}|g" "$APPSETTINGS"
sed -i "s|<jaeger-ui-endpoint>|${JAEGER_UI_URI}|g" "$APPSETTINGS"
sed -i "s|<grafana-dashboard-endpoint>|${GRAFANA_DASHBOARD_URI}|g" "$APPSETTINGS"

echo "Configuration environment variables in $APPSETTING complete."