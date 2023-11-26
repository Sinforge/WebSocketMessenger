using Moq;
using WebSocketMessenger.Core.Exceptions;
using WebSocketMessenger.Core.Interfaces.Repositories;
using WebSocketMessenger.Core.Models;
using WebSocketMessenger.UseCases.Services;

namespace UseCasesTest
{
    public class MessageServiceTests
    {
        [Fact]
        public async Task GetMessageByGroupAsyncTest_Success()
        {
            // Arrange
            Mock<IMessageRepository> mockMessageRepository = new Mock<IMessageRepository>();
            mockMessageRepository.Setup(m => m.GetGroupMessagesAsync(It.IsAny<Guid>())).ReturnsAsync(() => new List<Message>(1));
            Mock<IGroupRepository> mockGroupRepository = new Mock<IGroupRepository>();
            mockGroupRepository.Setup(m => m.IsGroupMember(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(() =>  true);

            MessageService messageService = new MessageService(mockMessageRepository.Object, mockGroupRepository.Object);

            // Act
            IEnumerable<int> result = await messageService.GetMessageByGroupAsync(It.IsAny<Guid>(), It.IsAny<Guid>());

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetMessageByGroupAsyncTest_Fail()
        {
            // Arrange
            Mock<IMessageRepository> mockMessageRepository = new Mock<IMessageRepository>();
            Mock<IGroupRepository> mockGroupRepository = new Mock<IGroupRepository>();
            mockGroupRepository.Setup(m => m.IsGroupMember(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(() => false);

            MessageService messageService = new MessageService(mockMessageRepository.Object, mockGroupRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<SharedException>(async () => await messageService.GetMessageByGroupAsync(It.IsAny<Guid>(), It.IsAny<Guid>()));
        }

        [Fact]
        public async Task GetMessageByIdAsync_Success()
        {
            // Arrange
            Guid userId = It.IsAny<Guid>();
            Mock<IMessageRepository> mockMessageRepository = new Mock<IMessageRepository>();
            mockMessageRepository.Setup(m => m.GetMessageByIdAsync(It.IsAny<int>())).ReturnsAsync(() => new Message { SenderId = userId, MessageContentType = 1, Content = "Aboba"});
            Mock<IGroupRepository> mockGroupRepository = new Mock<IGroupRepository>();

            MessageService messageService = new MessageService(mockMessageRepository.Object, mockGroupRepository.Object);

            // Act

            MessageDTO messageDTO = await messageService.GetMessageByIdAsync(It.IsAny<int>(), userId);

            // Assert
            Assert.NotNull(messageDTO);
        }

        [Fact]
        public async Task GetMessageByIdAsync_Fail_NotFound()
        {
            // Arrange
            Guid userId = It.IsAny<Guid>();
            Mock<IMessageRepository> mockMessageRepository = new Mock<IMessageRepository>();
            mockMessageRepository.Setup(m => m.GetMessageByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);
            Mock<IGroupRepository> mockGroupRepository = new Mock<IGroupRepository>();

            MessageService messageService = new MessageService(mockMessageRepository.Object, mockGroupRepository.Object);

            // Act & Assert

            SharedException e = await Assert.ThrowsAsync<SharedException>(async() => await messageService.GetMessageByIdAsync(It.IsAny<int>(), userId));
            Assert.Equal("Message not found", e.Message);
            Assert.Equal(400, e.StatusCode);
        }

        [Fact]
        public async Task GetMessageByIdAsync_Fail_NoAccess()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Mock<IMessageRepository> mockMessageRepository = new Mock<IMessageRepository>();
            mockMessageRepository.Setup(m => m.GetMessageByIdAsync(It.IsAny<int>())).ReturnsAsync(() => new Message {MessageContentType = 1, Content = "Aboba" });
            Mock<IGroupRepository> mockGroupRepository = new Mock<IGroupRepository>();

            MessageService messageService = new MessageService(mockMessageRepository.Object, mockGroupRepository.Object);

            // Act & Assert

            SharedException e = await Assert.ThrowsAsync<SharedException>(async () => await messageService.GetMessageByIdAsync(It.IsAny<int>(), userId));
            Assert.Equal("Not access for this message", e.Message);
            Assert.Equal(403, e.StatusCode);
        }


    }
}