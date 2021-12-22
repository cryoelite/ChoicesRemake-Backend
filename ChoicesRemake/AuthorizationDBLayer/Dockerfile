FROM "mcr.microsoft.com/mssql/server:2019-CU13-ubuntu-20.04"

MAINTAINER me
USER root

RUN mkdir -p ./data
COPY . ./data
RUN chmod +x ./data/run-initialization.sh
RUN chmod +x ./data/entrypoint.sh

ENV SA_PASSWORD Uxz5#2@1+7
ENV ACCEPT_EULA Y
ENV MSSQL_PID Express
ENV MSSQL_MEMORY_LIMIT_MB 1024

CMD /bin/bash ./data/entrypoint.sh
EXPOSE 1433