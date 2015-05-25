-- MySQL Script generated by MySQL Workbench
-- Mon May 25 19:30:52 2015
-- Model: New Model    Version: 1.0
SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema PurpleDatabase
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `PurpleDatabase` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci ;
USE `PurpleDatabase` ;

-- -----------------------------------------------------
-- Table `PurpleDatabase`.`server`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PurpleDatabase`.`server` ;

CREATE TABLE IF NOT EXISTS `PurpleDatabase`.`server` (
  `id` INT NOT NULL,
  `guid` VARCHAR(45) NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  `host` VARCHAR(45) NOT NULL,
  `port` INT NOT NULL,
  `currnet_player` INT NULL,
  `max_player` INT NOT NULL,
  `type` ENUM('Account', 'Lobby', 'Game', 'Multi', 'Monitoring') NOT NULL DEFAULT 'Multi',
  `local_ip` VARCHAR(45) NULL,
  `global_ip` VARCHAR(45) NULL,
  `timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
