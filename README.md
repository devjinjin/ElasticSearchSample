# ElasticSearchSample

## Docker

```
docker network create elastic

docker pull docker.elastic.co/elasticsearch/elasticsearch:8.1.3

docker run --name es-node01 --net elastic -p 9200:9200 -p 9300:9300 -t docker.elastic.co/elasticsearch/elasticsearch:8.1.3
```

 => 세번째 명령어를 실행하고 나면  다음과 같은 오류가 발생.
 ERROR: bootstrap checks failed
 max virtual memory areas vm.max_map_count [65530]  is too low, increase to at least [262144]
 
 => 윈도우 상에서 해당 옵션을 변경하기 위해서 cmd나 powershell에서 다음과 같은 명령어를 수행.

 wsl -d docker-desktop  # docker의 시스템 콘솔화면으로 진입이된다.
 sysctl -w vm.max_map_count=262144   # 이 명령으로 max_map_count값을 변경할 수 있다.
 이후, exit 로 빠져 나온다.

 => 영구적인 셋팅을 위해서는  /etc/sysctl.conf 파일의 내용을 변경한다.

 wsl -d docker-desktop  # docker의 시스템 콘솔화면으로 진입이된다.

 => vi 편집기를 사용하여 /etc/sysctl.conf 파일을 열고 아래 문장을 추가한다.
 vi /etc/sysctl.conf

 vm.max_map_count=262144
 => 이 환경변수를 사용하는 이유는 (https://www.gimsesu.me/elasticsearch-change-vm-max-map-count/) 참조

 이후, es-node01을 삭제후 다시 생성해도 되고, docker start es-node01 명령으로 컨테이너를 시작해도 된다.

 최초 실행될때만 출력되는 패스워드와 enrollment token 은 별도로 잘 저장해 두어야 한다.

 
### =========== 소스를 통한 테스트는 하기 내용 안해도 됨 =====================

그 다음, 별도의 터미널을 열어 kibana를 설치한다.

docker pull docker.elastic.co/kibana/kibana:8.1.3
docker run --name kib-01 --net elastic -p 5601:5601 docker.elastic.co/kibana/kibana:8.1.3

실행할 때 터미널에서 보이는 링크로 접속하여 enrollment token을 입력한다. 

i Kibana has not been configured.
Go to http://0.0.0.0:5601/?code=797908 to get started.

위와 같은 로그가 보이면 http://localhost:5601/?code=797908 과 같이 변경하여 URL을 입력하면 아래와 같은 enrollment token을 입력하는 화면이 나온다.

저장해둔 kibana용 enrollment token을 입력한 뒤 Configure Elastic 버튼을 클릭하면 환경설정 절차가 진행되고 최종 로그인 창이 나타난다.

아이디는 elastic , 패스워드는 위에서 저장해둔 패스워드를 이용하여 로그인을 하면 아래와 같은 화면이 나타난다.

패스워드 초기화를 위해서는 다음과 같은 명령어를 별도 터미널에 입력하면, 새로운 패스워드가 생성된다.

docker exec -it es-node01 /usr/share/elasticsearch/bin/elasticsearch-reset-password -u elastic
 

패스워드를 변경하기 위해서는 kibana 웹화면에 로그인 한 뒤, 오른쪽 상단에 profile 메뉴를 선택하면 패스워드 변경이 가능하다.

 

kibana를 위한 enrollment token을 재생성해야 하는 경우는 다음과 같은 명령을 사용한다.

docker exec -it es-node01 /usr/share/elasticsearch/bin/elasticsearch-create-enrollment-token -s kibana
 

elasticsearch, kibana 컨테이너 삭제와 네트워크 삭제는 다음과 같이 진행한다.

docker network rm elastic
docker rm es-node01
docker rm kib-01
