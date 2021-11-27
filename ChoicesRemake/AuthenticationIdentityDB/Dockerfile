FROM "mcr.microsoft.com/mssql/server:2019-CU13-ubuntu-20.04"

MAINTAINER me
USER root
RUN adduser --disabled-password --gecos '' millify \
    && adduser millify sudo \
    && echo '%sudo ALL=(ALL:ALL) ALL' >> /etc/sudoers

RUN mkdir -p /usr/src/app
RUN chown millify /usr/src/app
RUN mkdir -p ./ChoicesRemake/AuthenticationIdentityDB/SQLData:/var/opt/mssql/data
RUN chown 10001:0 ./ChoicesRemake/AuthenticationIdentityDB/SQLData:/var/opt/mssql/data
WORKDIR /usr/src/app
COPY . /usr/src/app
RUN chmod +x /usr/src/app/run-initialization.sh


ENV SA_PASSWORD Uxz5#2@1+7
ENV ACCEPT_EULA Y
ENV MSSQL_PID Express
ENV MSSQL_MEMORY_LIMIT_MB 1024

CMD /bin/bash ./entrypoint.sh
USER millify

EXPOSE 1433