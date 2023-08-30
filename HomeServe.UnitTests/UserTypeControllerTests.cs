using HomeServeHub.Controllers;
using HomeServeHub.DataAccess.UnitOfWork;
using HomeServeHub.Models;
using HomeServeHub.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HomeServe.UnitTests
{
    public class UserTypeControllerTests
    {
        [Fact]
        public void GetAllUserTypes_ReturnsOkResultWithCleanedUserTypes()
        {
            // Arrange
            var userTypes = new List<TbUserType>
            {
                new TbUserType
                {
                    UserTypeID = 1,
                    UserTypeName = "Admin",
                    UserTypeCurrentState = 1,
                    UserID = 1
                },
                new TbUserType
                {
                    UserTypeID = 2,
                    UserTypeName = "User",
                    UserTypeCurrentState = 1,
                    UserID = 2
                }
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uow => uow.TbUserType.GetAll(null, null)).Returns(userTypes);

            var controller = new UserTypeController(mockUnitOfWork.Object);

            // Act
            var result = controller.GetAllUserTypes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var cleanedUserTypes = okResult.Value as List<UserTypeDTO>;

            Assert.NotNull(cleanedUserTypes);
            Assert.Equal(userTypes.Count, cleanedUserTypes.Count);

            for (var i = 0; i < userTypes.Count; i++)
            {
                var expectedUserType = new UserTypeDTO
                {
                    UserTypeID = userTypes[i].UserTypeID,
                    UserTypeName = userTypes[i].UserTypeName,
                    UserTypeCurrentState = userTypes[i].UserTypeCurrentState,
                    UserID = userTypes[i].UserID
                };

                var actualUserType = cleanedUserTypes[i];

                Assert.Equal(expectedUserType.UserTypeID, actualUserType.UserTypeID);
                Assert.Equal(expectedUserType.UserTypeName, actualUserType.UserTypeName);
                Assert.Equal(expectedUserType.UserTypeCurrentState, actualUserType.UserTypeCurrentState);
                Assert.Equal(expectedUserType.UserID, actualUserType.UserID);
            }
        }

    }
}
