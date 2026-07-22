from fastapi import FastAPI

app = FastAPI(
    title="Nexus Books API",
    version="0.1.0",
)


@app.get("/health")
def health_check():
    return {"status": "healthy"}
