# Use a minimal base image with support for Linux applications
FROM --platform=linux/amd64 ubuntu:20.04

# Set the working directory
WORKDIR /Users/williamsimmons/Documents/GitHub/PaiShoDistributedGameClient/PaiShoDistributed

# Copy the Unity app into the container
COPY . .


# Make the application executable
RUN chmod +x ./PaiShoTestLinux.x86_64

Expose 5000

# Set the entry point for the container
CMD ["./PaiShoTestLinux.x86_64"]
