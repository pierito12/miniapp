pipeline {
    agent any

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOTNET_NOLOGO               = '1'
        APP_NAME                    = 'TodoApi'
        SOLUTION_FILE               = 'TodoApp.sln'
        PUBLISH_DIR                 = 'publish'
    }

    stages {

        stage('Clonar Repositorio') {
            steps {
                echo '================================================='
                echo ' ETAPA 1 — Clonar repositorio desde GitHub'
                echo '================================================='
                checkout scm
                echo "Repositorio clonado correctamente."
            }
        }

        stage('Restaurar Dependencias') {
            steps {
                echo '================================================='
                echo ' ETAPA 2 — dotnet restore'
                echo '================================================='
                sh 'dotnet restore ${SOLUTION_FILE}'
            }
        }

        stage('Compilar (Build)') {
            steps {
                echo '================================================='
                echo ' ETAPA 3 — dotnet build'
                echo '================================================='
                sh 'dotnet build ${SOLUTION_FILE} --no-restore --configuration Release'
            }
        }

        stage('Ejecutar Pruebas') {
            steps {
                echo '================================================='
                echo ' ETAPA 4 — dotnet test'
                echo '================================================='
                sh '''
                    dotnet test ${SOLUTION_FILE} \
                        --no-build \
                        --configuration Release \
                        --logger "trx;LogFileName=TestResults.trx" \
                        --collect:"XPlat Code Coverage"
                '''
            }
            post {
                always {
                    // Publicar resultados de pruebas en Jenkins
                    junit allowEmptyResults: true, testResults: '**/TestResults/*.trx'
                }
            }
        }

        stage('Publicar Aplicación') {
            steps {
                echo '================================================='
                echo ' ETAPA 5 — dotnet publish'
                echo '================================================='
                sh '''
                    dotnet publish TodoApi/TodoApi.csproj \
                        --no-build \
                        --configuration Release \
                        --output ${PUBLISH_DIR}
                '''
                echo "Artefactos publicados en: ${PUBLISH_DIR}/"
            }
        }

        stage('Archivar Artefactos') {
            steps {
                echo '================================================='
                echo ' ETAPA 6 — Archivar artefactos en Jenkins'
                echo '================================================='
                archiveArtifacts artifacts: "${PUBLISH_DIR}/**/*", fingerprint: true
                echo "Artefactos archivados exitosamente."
            }
        }
    }

    post {
        success {
            echo '✅ Pipeline completado exitosamente — Todos los stages pasaron.'
        }
        failure {
            echo '❌ Pipeline falló — Revisar los logs de las etapas anteriores.'
        }
        always {
            echo "Pipeline finalizado. Estado: ${currentBuild.currentResult}"
            cleanWs()
        }
    }
}
